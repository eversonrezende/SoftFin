using Microsoft.EntityFrameworkCore;
using SoftFin.Api.Data;
using SoftFin.Core.Handlers;
using SoftFin.Core.Models;
using SoftFin.Core.Requests.Transactions;
using SoftFin.Core.Responses;

namespace SoftFin.Api.Handlers;

public class TransactionHandler(AppDbContext _context) : ITransactionHandler
{
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        try
        {
            var transaction = new Transaction
            {
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.UtcNow,
                Amount = request.Amount,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Title = request.Title,
                Type = request.Type
            };

            await _context.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, 201, "Transação criada com sucesso");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Não foi possível criar a transação");
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await _context.Transactions
                    .FirstOrDefaultAsync(x =>
                    x.UserId == request.UserId && x.Id == request.Id);

            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transação não encontrada");

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, message: "Transação excluída com sucesso");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Não foi possível excluir a transação");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await _context.Transactions
                    .FirstOrDefaultAsync(x => 
                        x.UserId == request.UserId && x.Id == request.Id);

            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transação não encontrada");

            return new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Não foi possível recuperar a transação");
        }
    }

    public Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionByPeriodRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        try
        {
            var transaction = await _context.Transactions
                            .FirstOrDefaultAsync(x => 
                            x.UserId == request.UserId && x.Id == request.Id);

            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transação não encontrada");

            transaction.CategoryId = request.CategoryId;
            transaction.Amount = request.Amount;
            transaction.Title = request.Title;
            transaction.Type = request.Type;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, message: "Transação atualizada com sucesso");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Não foi possível atualizar a transação");
        }
    }
}
