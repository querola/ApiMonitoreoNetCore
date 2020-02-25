using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using monitoreoApiNetCore.Entities;

namespace monitoreoApiNetCore.Contexts
{
    public class pn_importacion_wsIndraContext : DbContext
    {
        public pn_importacion_wsIndraContext(DbContextOptions<pn_importacion_wsIndraContext> options) : base(options) {}
        
        public DbSet<pn_importacion_wsIndra> pn_importacion_wsIndra { get; set; }        
    }
}