using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MURDoX.Core.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private IConfiguration configuration;
    public ApplicationDbContext CreateDbContext(string[] args = null)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>();
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        return new ApplicationDbContext(options.Options);
    }
}