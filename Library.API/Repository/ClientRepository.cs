using Library.API.DTOs;
using Library.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Repository;

public interface IClientRepository
{
    Task<List<ClientModel>> GetAllClient(CancellationToken ct);
    Task<ClientModel> GetClientById(Guid clientId, CancellationToken ct);
    Task CreateClient(ClientModel payload, CancellationToken ct);
    Task DeleteClient(Guid clientId, CancellationToken ct);
    Task UpdateClient(Guid clientId, ClientModel payload, CancellationToken ct);
}

public class ClientRepository : IClientRepository
{
    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    private readonly AppDbContext _context;

    public async Task<List<ClientModel>> GetAllClient(CancellationToken ct = default)
    {
        return await _context.Client
            .Where(d => d.DeletedAt == null)
            .ToListAsync(ct);
    }

    public async Task<ClientModel> GetClientById(Guid clientId, CancellationToken ct)
    {
        return await _context.Client
            .Where(d => d.DeletedAt == null && d.Id == clientId)
            .FirstAsync(ct);
    }

    public async Task CreateClient(ClientModel payload, CancellationToken ct)
    {
        var client = await _context.Client.AddAsync(payload);

        if (client == null)
        {
            throw new Exception("Client not found");
        }

        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteClient(Guid clientId, CancellationToken ct)
    {
        var client = await _context.Client.FindAsync(clientId);

        if (client == null)
        {
            throw new Exception("Client not found");
        }

        _context.Client.Remove(client);

        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateClient(Guid clientId, ClientModel payload, CancellationToken ct = default)
    {
        var client = await _context.Client.FindAsync(clientId);

        if (client == null)
        {
            throw new Exception("Client not found");
        }

        client.Name = payload.Name;
        client.Email = payload.Email;

        await _context.SaveChangesAsync(ct);
    }
}
