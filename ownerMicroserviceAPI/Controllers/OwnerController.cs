using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ownerMicroservice.Application.Services;
using ownerMicroservice.Domain.Entities;
using ownerMicroservice.Domain.Services;
using ownerMicroservice.DTOs;

namespace ownerMicroserviceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OwnerController : ControllerBase
{
    private readonly OwnerService _ownerService;
    private readonly IValidator<ownerMicroservice.Domain.Entities.Owner> _validator;

    public OwnerController(OwnerService ownerService, IValidator<ownerMicroservice.Domain.Entities.Owner> validator)
    {
        _ownerService = ownerService;
        _validator = validator;
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("create")]
    [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Insert([FromBody] CreateOwnerDto dto, [FromHeader(Name = "userId")] int userId)
    {
        var owner = new ownerMicroservice.Domain.Entities.Owner
        {
            Name = dto.Name,
            FirstLastname = dto.FirstLastname,
            SecondLastname = dto.SecondLastname,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            DocumentNumber = dto.DocumentNumber,
            DocumentExtension = dto.DocumentExtension,
            Address = dto.Address
        };

        var validation = _validator.Validate(owner);
        if (validation.IsFailure)
        {
            return BadRequest(new ValidationErrorResponse
            {
                Message = "Validación fallida",
                Errors = validation.Errors
            });
        }

        var created = await _ownerService.Create(owner, userId);
        if (!created) return StatusCode(500, new { message = "Error al crear el dueño" });

        return CreatedAtAction(nameof(GetById),
            new { id = owner.Id },
            new SuccessResponse { Message = "Dueño creado exitosamente", Id = owner.Id });
    }

    [Authorize(Roles = "Manager")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ownerMicroservice.Domain.Entities.Owner>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Select()
    {
        var owners = await _ownerService.GetAll();
        return Ok(owners);
    }

    [Authorize(Roles = "Manager")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ownerMicroservice.Domain.Entities.Owner), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var owner = await _ownerService.GetById(id);
        if (owner == null) return NotFound(new { message = $"Dueño con ID {id} no encontrado" });
        return Ok(owner);
    }

    [Authorize(Roles = "Manager")]
    [HttpPut()]
    [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromBody] Owner owner, [FromHeader] int userId)
    {
        var existing = await _ownerService.GetById(owner.Id);
        if (existing == null) 
          return NotFound(new { message = $"Dueño con ID {owner.Id} no encontrado" });

        var validation = _validator.Validate(owner);
        if (validation.IsFailure)
        {
            return BadRequest(new ValidationErrorResponse
            {
                Message = "Validación fallida",
                Errors = validation.Errors
            });
        }

        var success = await _ownerService.Update(owner, userId);
        if (!success) return StatusCode(500, new { message = "Error al actualizar el dueño" });

        return Ok(new SuccessResponse { Message = "Dueño actualizado exitosamente", Id = owner.Id });
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteById(int id, [FromHeader] int userId)
    {
        var existing = await _ownerService.GetById(id);
        if (existing == null) return NotFound(new { message = $"Dueño con ID {id} no encontrado" });

        var success = await _ownerService.DeleteById(id, userId);
        if (!success) return NotFound(new { message = "Error al eliminar al dueño" });

        return Ok(new { message = "Dueño eliminado exitosamente" });
    }
}
