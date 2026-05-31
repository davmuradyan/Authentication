using System.Security.Claims;
using Authentication.Application.Auth;
using Authentication.Contracts.Auth.Dtos;
using Authentication.Contracts.Auth.Dtos.Creation;
using Authentication.Contracts.Auth.Dtos.Login;
using Authentication.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Endpoints.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await authService.Login(loginDto);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(new
        {
            result.AccessToken,
            result.RefreshToken
        });
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshDto refreshDto)
    {
        var result = await authService.Refresh(refreshDto);

        if (!result.Success)
            return Unauthorized(result.Message);

        return Ok(new
        {
            result.AccessToken,
            result.RefreshToken
        });
    }
    
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutDto logoutDto)
    {
        var result = await authService.Logout(logoutDto);

        if (!result)
            return BadRequest("Invalid or already revoked token.");

        return Ok("Logged out successfully.");
    }
    
    [HttpPost("logout-all")]
    [Authorize]
    public async Task<IActionResult> LogoutAll()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized("Invalid token.");

        await authService.LogoutAll(userId);

        return Ok("Logged out from all devices successfully.");
    }

    [HttpPost("invite")]
    [Authorize(Policy = $"{nameof(Policies.Company.Create)}")]
    public async Task<IActionResult> Invite([FromBody] InvitationDto invitationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.Invite(invitationDto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("activate")]
    public async Task<IActionResult> Activate([FromBody] ActivationDto activationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.Activate(activationDto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
