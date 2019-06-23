using InterviewWeb.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewWeb.Infrastructure
{

    public class SODbContext : DbContext
    {
        public SODbContext(DbContextOptions<SODbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }

    public class SOContextFactory
    {
        private readonly DbContextOptions<SODbContext> _options;

        public SOContextFactory(DbContextOptions<SODbContext> options)
        {
            _options = options;
        }

        public SODbContext GetNewDbContext()
        {
            return new SODbContext(_options);
        }
    }
}