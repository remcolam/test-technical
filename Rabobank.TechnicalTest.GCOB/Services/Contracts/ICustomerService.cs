using CSharpFunctionalExtensions;

namespace Rabobank.TechnicalTest.GCOB.Services.Contracts;

public interface ICustomerService
{
	Task<Result<Customer>> GetByIdentityAsync(int identity);
	Task<Result<Customer>> StoreAsync(Customer customer);
}
