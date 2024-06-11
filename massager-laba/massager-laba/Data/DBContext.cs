using massager_laba.Data.DTO;
using massager_laba.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace massager_laba.Data;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    public DbSet<UserModel> Users { get; set; }
    public DbSet<MessagerModel> MessagerModels { get; set; }
    public DbSet<MessageInfo> MessageInfos { get; set; }
}