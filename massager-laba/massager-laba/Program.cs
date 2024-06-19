using massager_laba.Data;
using massager_laba.Interface;
using massager_laba.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using massager_laba.Data.DTO;

var builder = WebApplication.CreateBuilder(args);

// Register services in the DI container.
builder.Services.AddControllers();
builder.Services.AddDbContext<DBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<WebSocketConnectionManager>();
builder.Services.AddTransient<IMeassagerService, MessagerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = TokenConfigurations.Issuer,
            ValidateAudience = true,
            ValidAudience = TokenConfigurations.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = TokenConfigurations.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<DBContext>();
    dbContext.Database.Migrate(); 
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var userId = context.Request.Query["userId"];
            if (string.IsNullOrWhiteSpace(userId))
            {
                context.Response.StatusCode = 400;
                return;
            }
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var connectionManager = context.RequestServices.GetRequiredService<WebSocketConnectionManager>();
            connectionManager.AddSocket(userId, webSocket);
            await HandleWebSocketCommunication(context, webSocket, connectionManager);
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
    else
    {
        await next();
    }
});

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

static async Task HandleWebSocketCommunication(HttpContext context, WebSocket webSocket, WebSocketConnectionManager connectionManager)
{
    var buffer = new byte[1024 * 4];
    WebSocketReceiveResult result;

    try
    {
        do
        {
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine("Received: " + receivedMessage);

                var message = JsonSerializer.Deserialize<MessagerSocetDTO>(receivedMessage);
                if (message != null)
                {
                    if (Guid.TryParse(message.FromUserId, out Guid fromUserId) && Guid.TryParse(message.ToUserId, out Guid toUserId))
                    {
                     
                        var messageService = context.RequestServices.GetRequiredService<IMeassagerService>();
                        await messageService.SendMessage(fromUserId, toUserId, message.Content, message.TypeMessage);

                       
                        var recipientSocket = connectionManager.GetSocketById(message.ToUserId);
                        if (recipientSocket != null && recipientSocket.State == WebSocketState.Open)
                        {
                            var responseMessage = JsonSerializer.Serialize(message);
                            await recipientSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(responseMessage)), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid GUID format in message.");
                    }
                }
            }
        } while (!result.CloseStatus.HasValue);
        
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception in WebSocket communication: " + ex.Message);
        await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Exception in WebSocket communication", CancellationToken.None);
    }
    finally
    {
        var userId = context.Request.Query["userId"];
        connectionManager.RemoveSocket(userId);
    }
}
