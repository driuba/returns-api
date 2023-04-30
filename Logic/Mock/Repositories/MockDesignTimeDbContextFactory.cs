using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Returns.Logic.Mock.Repositories;

[UsedImplicitly]
public class MockDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MockDbContext>
{
    public MockDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<MockDbContext>();

        builder.UseSqlite(
            Environment.ExpandEnvironmentVariables(@"DataSource=%HOME%/Repos/returns/uni/returns-api/Resources/Databases/mock.db;Mode=ReadWrite"),
            o => o.UseRelationalNulls()
        );

        return new MockDbContext(builder.Options);
    }
}
