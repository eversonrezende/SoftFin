using Microsoft.EntityFrameworkCore;
using SoftFin.Api.Data;
using SoftFin.Core.Handlers;
using SoftFin.Core.Models;
using SoftFin.Core.Requests.Categories;
using SoftFin.Core.Responses;

namespace SoftFin.Api.Handlers;

public class CategoryHandler(AppDbContext _context) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
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
            return new Response<Category?>(category, 201, "Categoria criada com sucesso");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Não foi possível criar a categoria");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category is null)
                return new Response<Category?>(null, 404, "Categoria não encontrada");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return new Response<Category?>(category, message: "Categoria excluída com sucesso");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Não foi possível excluír a categoria");
        }
    }

    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
    {
        try
        {
            var query = _context
            .Categories
            .AsNoTracking()
            .Where(x => x.UserId == request.UserId)
            .OrderBy(x => x.Title);

            var categories = await query
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Category>>(
                categories,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Category>>(null, 500, "Não foi possível consultar as categorias");
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return category is null
                ? new Response<Category?>(null, 404, "Categoria não encontrada")
                : new Response<Category?>(category);
        }
        catch
        {
            return new Response<Category?>(null, 500, "Não foi possível recuperar a categoria");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await _context.Categories
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category is null)
                return new Response<Category?>(null, 404, "Categoria não encontrada");

            category.Title = request.Title;
            category.Description = request.Description;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return new Response<Category?>(category, message: "Categoria atualizada com sucesso");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Não foi possível atualizar a categoria");
        }
    }
}
