using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Functions.Workers;

public interface INewsCategoriesRepository
{
    Task SaveCategoriesToNewsAsync(Guid newsId, List<Category> newsCategories);
}

public class NewsCategoriesRepository : INewsCategoriesRepository
{
    private readonly string _connectionString;

    public NewsCategoriesRepository(IOptions<ConnectionOptions> connectionOptions)
    {
        Ensure.NotNull(connectionOptions, nameof(connectionOptions));

        _connectionString = connectionOptions.Value.SqlWriter;
    }

    public async Task SaveCategoriesToNewsAsync(Guid newsId, List<Category> newsCategories)
    {
        var queryText = "INSERT INTO NewsTags VALUES(@tagId, @newsId)";

        var queryData = newsCategories.Select(x => new
        {
            NewsId = newsId,
            TagId = x.Id
        });
        
        using var sqlConnection = new SqlConnection(_connectionString);
        await sqlConnection.ExecuteAsync(queryText, queryData);
    }
}