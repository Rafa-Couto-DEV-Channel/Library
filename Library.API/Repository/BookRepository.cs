using Library.API.DTOs;
using Library.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Repository;

public interface IBookRepository
{
    Task<List<BookModel>> GetAllBook(CancellationToken ct);
    Task<BookModel> GetBookById(Guid bookId, CancellationToken ct);
    Task CreateBook(BookModel payload, CancellationToken ct);
    Task DeleteBook(Guid bookId, CancellationToken ct);
    Task UpdateBook(Guid bookId, BookModel payload, CancellationToken ct);
}

public class BookRepository : IBookRepository
{
    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    private readonly AppDbContext _context;

    public async Task<List<BookModel>> GetAllBook(CancellationToken ct = default)
    {
        return await _context.Book
            .Where(d => d.DeletedAt == null)
            .ToListAsync(ct);
    }

    public async Task<BookModel> GetBookById(Guid bookId, CancellationToken ct)
    {
        return await _context.Book
            .Where(d => d.DeletedAt == null && d.Id == bookId)
            .FirstAsync(ct);
    }

    public async Task CreateBook(BookModel payload, CancellationToken ct)
    {
        var book = await _context.Book.AddAsync(payload);

        if (book == null)
        {
            throw new Exception("Book not found");
        }

        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteBook(Guid bookId, CancellationToken ct)
    {
        var book = await _context.Book.FindAsync(bookId);

        if (book == null)
        {
            throw new Exception("Book not found");
        }

        _context.Book.Remove(book);

        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateBook(Guid bookId, BookModel payload, CancellationToken ct = default)
    {
        var book = await _context.Book.FindAsync(bookId);

        if (book == null)
        {
            throw new Exception("Book not found");
        }

        book.Title = payload.Title;
        book.Author = payload.Author;

        await _context.SaveChangesAsync(ct);
    }
}
