using CSharpFunctionalExtensions;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Services.Contracts;
using System.Collections.Concurrent;

namespace Rabobank.TechnicalTest.GCOB.Services;

public class InMemoryCountryRepository : ICountryRepository
{
	private ConcurrentDictionary<int, CountryDto> Countries { get; } = new ConcurrentDictionary<int, CountryDto>();
	private ConcurrentDictionary<string, CountryDto> CountriesByName { get; set; }
	private ILogger<InMemoryCountryRepository> _logger;

	public InMemoryCountryRepository(ILogger<InMemoryCountryRepository> logger)
	{
		_logger = logger;
		Countries = new ConcurrentDictionary<int, CountryDto>(
			new[]
			{
				new CountryDto { Id = 1, Name = "Netherlands" },
				new CountryDto { Id = 2, Name = "Poland" },
				new CountryDto { Id = 3, Name = "Ireland" },
				new CountryDto { Id = 4, Name = "South Afrcia" },
				new CountryDto { Id = 5, Name = "India" },
			}.ToDictionary(c => c.Id));
		CountriesByName = new ConcurrentDictionary<string, CountryDto>(Countries.Values.ToDictionary(c => c.Name, StringComparer.OrdinalIgnoreCase));
	}

	public Task<Result<CountryDto>> GetAsync(int identity)
	{
		_logger.LogDebug($"Get Country with identity {identity}");

		if (!Countries.ContainsKey(identity))
		{
			Task.FromResult(Result.Failure<CountryDto>($"Country not found by identity {identity}"));
		}

		_logger.LogDebug($"Found Country with identity {identity}");
		return Task.FromResult(Result.Success(Countries[identity]));
	}

	public Task<Result<IEnumerable<CountryDto>>> GetAllAsync()
	{
		_logger.LogDebug($"Get all Countries");

		return Task.FromResult(Result.Success(Countries.Select(x => x.Value)));
	}

	public Task<Result<CountryDto>> GetByNameAsync(string name)
	{
		_logger.LogDebug($"Get Country with name {name}");

		if (!CountriesByName.ContainsKey(name))
		{
			Task.FromResult(Result.Failure<CountryDto>($"Country not found by name {name}"));
		}

		_logger.LogDebug($"Found Country with name {name}");
		return Task.FromResult(Result.Success(CountriesByName[name]));
	}

}

