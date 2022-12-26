using System;
using System.Linq;
using System.Threading.Tasks;
using Itan.Common;
using Itan.Wrappers;

namespace Itan.Functions.Workers;

public class Function5Worker : IFunction5Worker
{
    private readonly ISerializer _serializer;
    private readonly ICategoriesProvider _categoriesIdProvider;
    private readonly INewsCategoriesRepository _newsCategoriesRepository;

    public Function5Worker(ISerializer serializer,
        ICategoriesProvider categoriesIdProvider,
        INewsCategoriesRepository newsCategoriesRepository)
    {
        _serializer = serializer;
        _categoriesIdProvider = categoriesIdProvider;
        _newsCategoriesRepository = newsCategoriesRepository;
    }

    public async Task Run(string queueItem)
    {
        var channelToDownload = _serializer.Deserialize<NewsCategories>(queueItem);

        var normalizedCategories = channelToDownload.Categories.Select(x => x.ToLowerInvariant().Trim());
        var newsCategories = await _categoriesIdProvider.GetOrCreateByNamesAsync(normalizedCategories.ToList());
        await _newsCategoriesRepository.SaveCategoriesToNewsAsync(channelToDownload.NewsId, newsCategories);
    }
}

public class Category
{
    public Guid Id { get; set; }
    public string Text { get; set; }
}