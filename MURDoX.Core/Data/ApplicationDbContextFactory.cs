#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

#endregion

namespace MURDoX.Core.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private IConfiguration configuration;

        public ApplicationDbContext CreateDbContext(string[] args = null)
        {
            DbContextOptionsBuilder<ApplicationDbContext>?
                options = new();
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            return new ApplicationDbContext(options.Options);
        }
    }
}