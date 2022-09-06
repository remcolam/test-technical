using CSharpFunctionalExtensions;
using Rabobank.TechnicalTest.GCOB.Dtos;

namespace Rabobank.TechnicalTest.GCOB.Services.Contracts;

public interface IAddressRepository
{
	Task<Result<int>> GenerateIdentityAsync();
	Task<Result<int>> InsertAsync(AddressDto address);
	Task<Result<AddressDto>> GetAsync(int identity);
	Task<Result> UpdateAsync(AddressDto address);
}
