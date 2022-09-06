using CSharpFunctionalExtensions;
using Rabobank.TechnicalTest.GCOB.Dtos;

namespace Rabobank.TechnicalTest.GCOB.Services.Contracts;

public interface ICustomerRepository
{
	Task<Result<int>> InsertAsync(CustomerDto customer);
	Task<Result<CustomerDto>> GetAsync(int identity);
	Task<Result> UpdateAsync(CustomerDto customer);
}
