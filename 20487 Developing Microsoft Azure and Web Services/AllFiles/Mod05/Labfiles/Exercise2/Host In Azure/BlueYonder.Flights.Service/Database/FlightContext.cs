using System;
using Microsoft.EntityFrameworkCore;
using BlueYonder.Flights.Service.Models;

namespace BlueYonder.Flights.Service.Database
{
    public class FlightContext : DbContext
    {
        public FlightContext()
        {
        }
		
        public FlightContext(DbContextOptions<FlightContext> options)
        : base(options)
        {
        }
		
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var dbConnectionString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_dbConnectionString");
            var dbConnectionString = Environment.GetEnvironmentVariable("dbConnectionString");
            dbConnectionString = "Server=tcp:blueyonder05-bmvb.database.windows.net,1433;Initial Catalog=BlueYonder.Flights.Lab05;Persist Security Info=False;User ID=BlueYonderAdmin;Password=Pa$$w0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(dbConnectionString);
            }
        }
		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>().HasMany(flight => flight.Travelers);
        }
		
        public DbSet<Flight> Flights { get; set; }
		public DbSet<Traveler> Travelers { get; set; }
    }
}
