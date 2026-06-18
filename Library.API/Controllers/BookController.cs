using FluentValidation;
using Library.API.DTOs;
using Library.API.Models;
using Library.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controller;

[ApiController]
[Route("api/book")]
public class BookController : ControllerBase
{
    public BookController(
        IBookRepository bookRepository,
        IValidator<BookDto> bookValidator,
        ILogger<BookController> logger
    )
    {
        this._bookRepository = bookRepository;
        this._bookValidator = bookValidator;
        this._logger = logger;

    }

    private readonly IBookRepository _bookRepository;
    private readonly IValidator<BookDto> _bookValidator;
    private readonly ILogger<BookController> _logger;

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] BookDto request, CancellationToken ct)
    {
        var validation = await _bookValidator.ValidateAsync(request, ct);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var book = new BookModel
        {
            Author = request.Author,
            Description = request.Description,
            Title = request.Title
        };

        try
        {
            await _bookRepository.CreateBook(book, ct);

            return Ok(new { message = "Book added successful" });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500, new
            {
                message = ex.Message,
            });
        }

    }

    [HttpGet]
    public async Task<IActionResult> GetAllBook(CancellationToken ct)
    {
        try
        {
            var response = await _bookRepository.GetAllBook(ct);

            return Ok(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500, new
            {
                message = ex.Message,
            });
        }

    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBookById(Guid id, CancellationToken ct)
    {
        try
        {
            var response = await _bookRepository.GetBookById(id, ct);

            return Ok(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500, new
            {
                message = ex.Message,
            });
        }

    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id, CancellationToken ct)
    {
        try
        {
            await _bookRepository.DeleteBook(id, ct);

            return Ok(new { message = "Deleted successful" });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500, new
            {
                message = ex.Message,
            });
        }

    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBook(Guid id, [FromBody] BookDto request, CancellationToken ct)
    {
        var book = new BookModel
        {
            Author = request.Author,
            Description = request.Description,
            Title = request.Title
        };

        try
        {
            await _bookRepository.UpdateBook(id, book, ct);

            return Ok(new { message = "Updated successful" });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500, new
            {
                message = ex.Message,
            });
        }

    }

}