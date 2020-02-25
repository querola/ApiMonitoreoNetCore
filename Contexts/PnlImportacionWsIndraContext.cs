using Microsoft.EntityFrameworkCore;
using monitoreoApiNetCore.Entities;

namespace monitoreoApiNetCore.Contexts
{
    public class PnlImportacionWsIndraContext : DbContext
    {
        public PnlImportacionWsIndraContext(DbContextOptions<HistorialDbContext> options) : base(options) {}
        
        public DbSet<PnImportacionWsIndra> PnlImportacionWsIndra { get; set; }        
    }
}