using CSharpFunctionalExtensions;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Services.Contracts;
using System.Collections.Concurrent;

namespace Rabobank.TechnicalTest.GCOB.Services;

public class InMemoryAddressRepository : IAddressRepository
{
	private ConcurrentDictionary<int, AddressDto> Addresses { get; } = new ConcurrentDictionary<int, AddressDto>();
	private ILogger<InMemoryAddressRepository> _logger;

	public InMemoryAddressRepository(ILogger<InMemoryAddressRepository> logger)
	{
		_logger = logger;
	}

	public Task<Result<int>> GenerateIdentityAsync()
	{
		lock (this.Addresses)
		{
			_logger.LogDebug("Generating Address identity");

			return Task.Run(() =>
			{
				if (this.Addresses.Count == 0)
				{
					return Result.Success(1);
				}

				var x = this.Addresses.Values.Max(c => c.Id);
				return Result.Success(++x);
			});
		}
	}

	private async Task<Result<int>> GetIdentityIfNotSetAsync(AddressDto address)
	{
		if (address.Id > 0)
		{
			return Result.Success(address.Id);
		}

		return await GenerateIdentityAsync();
	}

	public Task<Result<int>> InsertAsync(AddressDto address)
	{
		lock (this.Addresses)
		{
			return GetIdentityIfNotSetAsync(address)
				.Ensure((identity) => !this.Addresses.ContainsKey(identity), $"Cannot insert address with identity '{address.Id}' as it already exists in the collection")
				.Tap((identity) => this.Addresses.TryAdd(identity, address))
				.Tap((identity) => _logger.LogDebug($"New address inserted [ID:{identity}]. There are now {this.Addresses.Count} legal entities in the store."));
		}
	}

	public Task<Result<AddressDto>> GetAsync(int identity)
	{
		_logger.LogDebug($"FindMany Customers with identity {identity}");

		if (!this.Addresses.ContainsKey(identity))
		{
			return Task.FromResult(Result.Failure<AddressDto>("Address not found"));
		}

		_logger.LogDebug($"Found Customer with identity {identity}");
		return Task.FromResult(Result.Success(this.Addresses[identity]));
	}

	public Task<Result> UpdateAsync(AddressDto address)
	{
		if (!this.Addresses.ContainsKey(address.Id))
		{
			return Task.FromResult(Result.Failure($"Cannot update address with identity '{address.Id}' as it doesn't exist"));
		}

		Addresses[address.Id] = address;
		_logger.LogDebug($"New address updated [ID:{address.Id}].");

		return Task.FromResult(Result.Success());
	}
}
