using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Functions.Workers;

public class CategoriesProvider : ICategoriesProvider
{
    private readonly string _connectionString;

    public CategoriesProvider(IOptions<ConnectionOptions> connectionOptions)
    {
        Ensure.NotNull(connectionOptions, nameof(connectionOptions));

        _connectionString = connectionOptions.Value.SqlReader;
    }

    public async Task<List<Category>> GetOrCreateByNamesAsync(List<string> normalizedCategories)
    {
        var queryText = " if not exists(select top 1 * from Tags where Text=@text)\n" +
                        " begin\n" +
                        "     insert into Tags VALUES (NEWID(), @text)\n" +
                        " end\n" +
                        " select id, text from Tags where text=@text";

        using var sqlConnection = new SqlConnection(_connectionString);

        var categories = normalizedCategories.Select(text => sqlConnection.QuerySingle<Category>(queryText, new { text }));
        return categories.ToList();
    }
}