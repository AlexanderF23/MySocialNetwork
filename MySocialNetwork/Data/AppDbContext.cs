using Microsoft.EntityFrameworkCore;
using MySocialNetwork.Models;

namespace MySocialNetwork.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Post> Posts { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}