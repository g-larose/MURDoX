using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using MURDoX.Services.Services;

namespace MURDoX.Data.Factories
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args = null)
        {
            var ds = new DataService();
            var config = ds.GetApplicationConfig();
            var connStr = config.ConnectionString!;
            var options = new DbContextOptionsBuilder<AppDbContext>();

            options.UseNpgsql(connStr);
            
            return new AppDbContext(options.Options);
        }
    }
}
