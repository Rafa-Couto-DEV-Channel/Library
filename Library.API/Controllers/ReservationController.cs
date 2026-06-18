using FluentValidation;
using Library.API.DTOs;
using Library.API.Models;
using Library.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controller;

[ApiController]
[Route("api/reservation")]
public class ReservationController : ControllerBase
{
    public ReservationController(
        IReservationRepository reservationRepository,
        IValidator<ReservationDto> reservationValidator,
        ILogger<ReservationController> logger
    )
    {
        this._reservationRepository = reservationRepository;
        this._reservationValidator = reservationValidator;
        this._logger = logger;

    }

    private readonly IReservationRepository _reservationRepository;
    private readonly IValidator<ReservationDto> _reservationValidator;
    private readonly ILogger<ReservationController> _logger;

    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] ReservationDto request, CancellationToken ct)
    {
        var validation = await _reservationValidator.ValidateAsync(request, ct);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var reservation = new ReservationModel
        {
            ClientModelId = request.ClientId,
            Book = request.Book,
        };

        try
        {
            await _reservationRepository.CreateReservation(reservation, ct);

            return Ok(new { message = "Reservation added successful" });

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
    public async Task<IActionResult> GetAllReservation(CancellationToken ct)
    {
        try
        {
            var response = await _reservationRepository.GetAllReservation(ct);

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
    public async Task<IActionResult> GetReservationById(Guid id, CancellationToken ct)
    {
        try
        {
            var response = await _reservationRepository.GetReservationById(id, ct);

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
    public async Task<IActionResult> DeleteReservation(Guid id, CancellationToken ct)
    {
        try
        {
            await _reservationRepository.DeleteReservation(id, ct);

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
    public async Task<IActionResult> UpdateReservation(Guid id, [FromBody] ReservationDto request, CancellationToken ct)
    {      

        var reservation = new ReservationModel
        {
            ClientModelId = request.ClientId,
            Book = request.Book,
        };

        try
        {
            await _reservationRepository.UpdateReservation(id, reservation, ct);

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