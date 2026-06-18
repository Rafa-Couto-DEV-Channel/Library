using Library.API.DTOs;
using Library.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Repository;

public interface IReservationRepository
{
    Task<List<ReservationModel>> GetAllReservation(CancellationToken ct);
    Task<ReservationModel> GetReservationById(Guid reservationId, CancellationToken ct);
    Task<List<ReservationModel>> GetAllReservationByClientId(Guid clientId, CancellationToken ct);
    Task CreateReservation(ReservationModel payload, CancellationToken ct);
    Task DeleteReservation(Guid reservationId, CancellationToken ct);
    Task UpdateReservation(Guid reservationId, ReservationModel payload, CancellationToken ct);
}

public class ReservationRepository : IReservationRepository
{
    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    private readonly AppDbContext _context;

    public async Task<List<ReservationModel>> GetAllReservation(CancellationToken ct = default)
    {
        return await _context.Reservation
            .Where(d => d.DeletedAt == null)
            .ToListAsync(ct);
    }

    public async Task<ReservationModel> GetReservationById(Guid reservationId, CancellationToken ct)
    {
        return await _context.Reservation
            .Where(d => d.DeletedAt == null && d.Id == reservationId)
            .FirstAsync(ct);
    }

    public async Task<List<ReservationModel>> GetAllReservationByClientId(Guid clientId, CancellationToken ct)
    {
        return await _context.Reservation
            .Where(d => d.DeletedAt == null && d.Id == clientId)
            .ToListAsync(ct);
    }

    public async Task CreateReservation(ReservationModel payload, CancellationToken ct)
    {
        var reservation = await _context.Reservation.AddAsync(payload);

        if (reservation == null)
        {
            throw new Exception("Reservation not found");
        }

        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteReservation(Guid reservationId, CancellationToken ct)
    {
        var reservation = await _context.Reservation.FindAsync(reservationId);

        if (reservation == null)
        {
            throw new Exception("Reservation not found");
        }

        _context.Reservation.Remove(reservation);

        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateReservation(Guid reservationId, ReservationModel payload, CancellationToken ct = default)
    {
        var reservation = await _context.Reservation.FindAsync(reservationId);

        if (reservation == null)
        {
            throw new Exception("Reservation not found");
        }

        reservation.Client = payload.Client;
        reservation.Book = payload.Book;

        await _context.SaveChangesAsync(ct);
    }
}
