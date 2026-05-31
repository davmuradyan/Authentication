using Authentication.Application.CrudServices.Company.Interfaces;
using Authentication.Application.Services.Repositories.Company;
using Authentication.Contracts.Crud.Company.Dtos;
using Authentication.Contracts.Crud.Company.Results;

namespace Authentication.Application.CrudServices.Company.Implementations;

public class CompanyService(ICompanyRepository companyRepository) : ICompanyService
{
    public async Task<CreateCompanyResult> CreateCompany(CreateCompanyDto dto)
    {
        var result = new CreateCompanyResult {Success = false};
        // Check the parameters
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            result.Message = "Name is required.";
            return result;
        }
        
        // Check if there is a company with the same name.
        if (await companyRepository.ExistsWithName(dto.Name))
        {
            result.Message = "There is already a company with the same name.";
            return result;
        }
        
        var company = Domain.Entities.Company.Company.Create(Guid.NewGuid() ,dto.Name);
        var createdCompany = await companyRepository.Create(company);
        
        if (createdCompany is null)
        {
            result.Message = "Failed to create company.";
            return result;
        }
        
        result.Success = true;
        result.CompanyId = createdCompany.Id;
        result.CompanyName = createdCompany.Name;
        return result;
    }
}