using System.Security.Claims;
using Authentication.Application.CrudServices.Company;
using Authentication.Application.CrudServices.Company.Interfaces;
using Authentication.Contracts.Crud.Company.Dtos;
using Authentication.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Endpoints.Controllers;

[ApiController]
[Route("api/v1/company")]
[Authorize]
public class CompanyController(ICompanyService companyService) : ControllerBase
{
    /// <summary>
    /// Creates a new company. Only accessible to Global Admin or users with Company.Create permission.
    /// </summary>
    [HttpPost("create")]
    [Authorize(Policy = $"{nameof(Policies.Company.Create)}")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto dto)
    {
        // Validate input
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await companyService.CreateCompany(dto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}