using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using monitoreoApiNetCore.Entities;

namespace monitoreoApiNetCore.Contexts
{
    public class HistorialDbContext : DbContext
    {
        public HistorialDbContext(DbContextOptions<HistorialDbContext> options) : base(options) { }
        public DbSet<Historial> Historial { get; set; }
    }
}