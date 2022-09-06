using CSharpFunctionalExtensions;
using Rabobank.TechnicalTest.GCOB.Dtos;

namespace Rabobank.TechnicalTest.GCOB.Services.Contracts;

public interface ICountryRepository
{
	Task<Result<CountryDto>> GetAsync(int identity);
	Task<Result<IEnumerable<CountryDto>>> GetAllAsync();
	Task<Result<CountryDto>> GetByNameAsync(string name);
}