using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itan.Functions.Workers;

public interface ICategoriesProvider
{
    Task<List<Category>> GetOrCreateByNamesAsync(List<string> normalizedCategories);
}