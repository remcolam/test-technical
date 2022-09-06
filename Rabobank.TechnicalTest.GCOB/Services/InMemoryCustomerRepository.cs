using CSharpFunctionalExtensions;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Services.Contracts;
using System.Collections.Concurrent;

namespace Rabobank.TechnicalTest.GCOB.Services;

public class InMemoryCustomerRepository : ICustomerRepository
{
	private ConcurrentDictionary<int, CustomerDto> Customers { get; } = new ConcurrentDictionary<int, CustomerDto>();
	private ILogger<InMemoryCustomerRepository> _logger;

	public InMemoryCustomerRepository(ILogger<InMemoryCustomerRepository> logger)
	{
		_logger = logger;
	}

	private Task<Result<int>> GenerateIdentityAsync()
	{
		lock (Customers)
		{
			_logger.LogDebug("Generating Customer identity");
			return Task.Run(() =>
			{
				if (Customers.Count == 0)
				{
					return Result.Success(1);
				}

				var x = this.Customers.Values.Max(c => c.Id);
				return Result.Success(++x);
			});
		}
	}

	private async Task<Result<int>> GetIdentityIfNotSetAsync(CustomerDto customer)
	{
		if (customer.Id > 0)
		{
			return Result.Success(customer.Id);
		}

		return await GenerateIdentityAsync();
	}

	public Task<Result<int>> InsertAsync(CustomerDto customer)
	{
		lock (this.Customers)
		{
			return GetIdentityIfNotSetAsync(customer)
				.Ensure((identity) => !this.Customers.ContainsKey(identity), $"Cannot insert customer with identity '{customer.Id}' as it already exists in the collection")
				.Tap((identity) => this.Customers.TryAdd(identity, customer))
				.Tap((identity) => _logger.LogDebug($"New customer inserted [ID:{identity}]. There are now {this.Customers.Count} legal entities in the store."));
		}
	}

	public Task<Result<CustomerDto>> GetAsync(int identity)
	{
		_logger.LogDebug($"FindMany Customers with identity {identity}");

		if (!this.Customers.ContainsKey(identity))
		{
			return Task.FromResult(Result.Failure<CustomerDto>("Customer not found"));
		}

		_logger.LogDebug($"Found Customer with identity {identity}");
		return Task.FromResult(Result.Success(this.Customers[identity]));
	}

	public Task<Result> UpdateAsync(CustomerDto customer)
	{
		if (!this.Customers.ContainsKey(customer.Id))
		{
			return Task.FromResult(Result.Failure($"Cannot update customer with identity '{customer.Id}' as it doesn't exist"));
		}

		this.Customers[customer.Id] = customer;
		_logger.LogDebug($"New customer updated [ID:{customer.Id}].");

		return Task.FromResult(Result.Success());
	}
}
