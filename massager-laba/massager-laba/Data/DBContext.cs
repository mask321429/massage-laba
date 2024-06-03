using massager_laba.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace massager_laba.Data;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options) { }
    public DbSet<UserModel> Users { get; set; }
}

