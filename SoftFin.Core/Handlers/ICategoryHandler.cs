using SoftFin.Core.Models;
using SoftFin.Core.Requests.Categories;
using SoftFin.Core.Responses;

namespace SoftFin.Core.Handlers;

public interface ICategoryHandler
{
    Task<Response<Category>> CreateAsync(CreateCategoryRequest request);
    Task<Response<Category>> UpdateAsync(UpdateCategoryRequest request);
    Task<Response<Category>> DeleteAsync(DeleteCategoryRequest request);
    Task<Response<Category>> GetByIdAsync(GetCategoryByIdRequest request);
    Task<Response<List<Category>>> GetAllAsync(GetAllCategoriesRequest request);
}
