using Authentication.Contracts.Crud.Company.Dtos;
using Authentication.Contracts.Crud.Company.Results;

namespace Authentication.Application.CrudServices.Company.Interfaces;

public interface ICompanyService
{
    Task<CreateCompanyResult> CreateCompany(CreateCompanyDto dto);
    
}