using Microsoft.AspNetCore.Mvc;
using Washing.DTOs;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IGenericRepository<User> _repository;

    public UsersController(IGenericRepository<User> repository)
    {
        _repository = repository;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserResponseDto>> Register(RegisterUserDto dto)
    {
        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role,
            Balance = 0
        };

        var created = await _repository.AddAsync(user);
        var response = new UserResponseDto(created.Id, created.FullName, created.Email, created.Role, created.Balance);
        return CreatedAtAction(nameof(Register), new { id = created.Id }, response);
    }
}
