using SoftFin.Api.Data;
using SoftFin.Core.Handlers;
using SoftFin.Core.Models;
using SoftFin.Core.Requests.Categories;
using SoftFin.Core.Responses;

namespace SoftFin.Api.Handlers;

public class CategoryHandler(AppDbContext _context) : ICategoryHandler
{
    public async Task<Response<Category>> CreateAsync(CreateCategoryRequest request)
    {
        // Instanciar a classe Category
        var category = new Category
        {
            UserId = request.UserId,
            Title = request.Title,
            Description = request.Description
        };

        // Adicionar a categoria ao banco de dados
        await _context.Categories.AddAsync(category);
        
        // Salvar as mudanças no banco de dados
        await _context.SaveChangesAsync();

        // Retornar a categoria criada
        return new Response<Category>(category);
    }

    public Task<Response<Category>> DeleteAsync(DeleteCategoryRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Category>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Category>> UpdateAsync(UpdateCategoryRequest request)
    {
        throw new NotImplementedException();
    }
}
