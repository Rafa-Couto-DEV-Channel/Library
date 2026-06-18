using FluentValidation;
using Library.API.DTOs;
using Library.API.Models;
using Library.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controller;

[ApiController]
[Route("api/client")]
public class ClientController : ControllerBase
{
    public ClientController(
        IClientRepository clientRepository,
        IValidator<ClientDto> clientValidator,
        ILogger<ClientController> logger
    )
    {
        this._clientRepository = clientRepository;
        this._clientValidator = clientValidator;
        this._logger = logger;

    }

    private readonly IClientRepository _clientRepository;
    private readonly IValidator<ClientDto> _clientValidator;
    private readonly ILogger<ClientController> _logger;

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] ClientDto request, CancellationToken ct)
    {
        var validation = await _clientValidator.ValidateAsync(request, ct);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var client = new ClientModel
        {
            Name = request.Name,
            Email = request.Email,
        };

        try
        {
            await _clientRepository.CreateClient(client, ct);

            return Ok(new { message = "Client added successful" });

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
    public async Task<IActionResult> GetAllClient(CancellationToken ct)
    {
        try
        {
            var response = await _clientRepository.GetAllClient(ct);

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
    public async Task<IActionResult> GetClientById(Guid id, CancellationToken ct)
    {
        try
        {
            var response = await _clientRepository.GetClientById(id, ct);

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
    public async Task<IActionResult> DeleteClient(Guid id, CancellationToken ct)
    {
        try
        {
            await _clientRepository.DeleteClient(id, ct);

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
    public async Task<IActionResult> UpdateClient(Guid id, [FromBody] ClientDto request, CancellationToken ct)
    {
        var validation = await _clientValidator.ValidateAsync(request, ct);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var client = new ClientModel
        {
            Name = request.Name,
            Email = request.Email,
        };
        
        try
        {
            await _clientRepository.UpdateClient(id, client, ct);

            return Ok(new { message = "Updated successful" });

        } catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500, new {
                message = ex.Message,
            });
        }
        
    }

}