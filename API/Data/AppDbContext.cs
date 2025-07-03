using System;
using API.Entity;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet <AppUser> Users { get; set; }
}
