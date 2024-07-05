﻿using SoftFin.Core.Handlers;
using SoftFin.Core.Models;
using SoftFin.Core.Requests.Categories;
using SoftFin.Core.Responses;
using System.Net.Http.Json;

namespace SoftFin.Web.Handlers;

public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);

    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/categories", request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
            ?? new Response<Category?>(null, 400, "Não foi possível criar a categoria");
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        var result = await _client.DeleteAsync($"v1/categories/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
            ?? new Response<Category?>(null, 400, "Não foi possível excluir a categoria");
    }

    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
        => await _client.GetFromJsonAsync<PagedResponse<List<Category>>>("v1/categories")
        ?? new PagedResponse<List<Category>>(null, 400, "Não foi possível recuperar as categorias");

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
        => await _client.GetFromJsonAsync<Response<Category?>>($"v1/categories/{request.Id}")
        ?? new Response<Category?>(null, 400, "Não foi possível recuperar a categoria");


    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        var result = await _client.PutAsJsonAsync($"v1/categories/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
            ?? new Response<Category?>(null, 400, "Não foi possível atualizar a categoria");
    }
}