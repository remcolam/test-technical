using CSharpFunctionalExtensions;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Services.Contracts;

namespace Rabobank.TechnicalTest.GCOB.Services;

public class CustomerService : ICustomerService
{
	private readonly ILogger<CustomerService> _logger;
	private readonly ICustomerRepository _customerRepository;
	private readonly IAddressRepository _addressRepository;
	private readonly ICountryRepository _countryRepository;

	public CustomerService(ILogger<CustomerService> logger
	, ICustomerRepository customerRepository
	, IAddressRepository addressRepository
	, ICountryRepository countryRepository)
	{
		_logger = logger;
		_customerRepository = customerRepository;
		_addressRepository = addressRepository;
		_countryRepository = countryRepository;
	}


	public async Task<Result<Customer>> GetByIdentityAsync(int identity)
	{
		return await _customerRepository.GetAsync(identity)
			.Bind(customer => _addressRepository.GetAsync(customer.AddressId)
				.Bind(address => _countryRepository.GetAsync(address.CountryId)
					.Map(country => Customer.Map(customer, address, country))));
	}

	public async Task<Result<Customer>> StoreAsync(Customer customer)
	{
		return await _countryRepository.GetByNameAsync(customer.Country)
			.Bind(country => StoreAddressAsync(customer, country))
			.Bind(address => StoreCustomerAsync(customer, address))
			.Tap(storedCustomer => customer.Id = storedCustomer.Id)
			.Map(_ => customer);
	}

	private async Task<Result<CustomerDto>> StoreCustomerAsync(Customer customerModel, AddressDto address)
	{
		var customer = customerModel.CopyTo(new CustomerDto { AddressId = address.Id });

		return await _customerRepository.InsertAsync(customer)
			.Tap(identity => customer.Id = identity)
			.Map(_ => customer);
	}

	private async Task<Result<AddressDto>> StoreAddressAsync(Customer customer, CountryDto country)
	{
		var address = customer.CopyTo(new AddressDto { CountryId = country.Id });

		return await _addressRepository.InsertAsync(address)
			.Tap(identity => address.Id = identity)
			.Map(_ => address);
	}
}
