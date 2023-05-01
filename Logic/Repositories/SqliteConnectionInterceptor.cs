using System.Data.Common;
using System.Globalization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Returns.Logic.Repositories;

public class SqliteConnectionInterceptor : DbConnectionInterceptor
{
    private static readonly StringComparer _comparer = StringComparer.Create(
        CultureInfo.InvariantCulture,
        CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
    );

    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        Initialize(connection);

        base.ConnectionOpened(connection, eventData);
    }

    public override Task ConnectionOpenedAsync(
        DbConnection connection,
        ConnectionEndEventData eventData,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        Initialize(connection);

        return base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

    private static void Initialize(DbConnection connection)
    {
        if (connection is not SqliteConnection sqliteConnection)
        {
            return;
        }

        sqliteConnection.CreateCollation(
            "NOCASE",
            _comparer.Compare
        );

        sqliteConnection.CreateFunction(
            "instr",
            (string? a, string? b) =>
                a is not null &&
                b is not null &&
                CultureInfo.InvariantCulture.CompareInfo.IndexOf(a, b, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) != -1
        );
    }
}
