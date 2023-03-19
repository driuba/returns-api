using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Returns.Logic.Repositories;

public class ReturnDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ReturnDbContext>
{
    public ReturnDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ReturnDbContext>();

        builder.UseSqlite(
            Environment.ExpandEnvironmentVariables(@"DataSource=%HOME%/Repos/returns/uni/databases/returns.db;Mode=ReadWrite"),
            o =>
            {
                o.UseRelationalNulls();
            }
        );

        return new ReturnDbContext(builder.Options, default!);
    }
}
