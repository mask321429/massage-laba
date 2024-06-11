using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace massager_laba.Services;

public class WebSocketConnectionManager
{
    private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

    public void AddSocket(string userId, WebSocket socket)
    {
        _sockets.TryAdd(userId, socket);
    }

    public WebSocket GetSocketById(string userId)
    {
        _sockets.TryGetValue(userId, out var socket);
        return socket;
    }

    public void RemoveSocket(string userId)
    {
        if (_sockets.TryRemove(userId, out var socket))
        {
            CloseSocket(socket).Wait();
        }
    }

    public async Task CloseSocket(WebSocket socket)
    {
        if (socket.State == WebSocketState.Open)
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }
    }
}