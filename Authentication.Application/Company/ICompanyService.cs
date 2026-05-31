using Authentication.Contracts.Company.Dtos;
using Authentication.Contracts.Company.Results;

namespace Authentication.Application.Company;

public interface ICompanyService
{
    Task<CreateCompanyResult> CreateCompany(CreateCompanyDto dto);
}