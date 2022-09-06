using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rabobank.TechnicalTest.GCOB.Services;
using Rabobank.TechnicalTest.GCOB.Services.Contracts;

namespace Rabobank.TechnicalTest.GCOB.Tests.Services;

[TestClass]
public class CustomerServiceTest
{
	private ICustomerService _customerService;
	private Customer _existingCustomer;


	[TestInitialize]
	public async Task Initialize()
	{
		_customerService = new CustomerService(Mock.Of<ILogger<CustomerService>>()
			, new InMemoryCustomerRepository(Mock.Of<ILogger<InMemoryCustomerRepository>>())
			, new InMemoryAddressRepository(Mock.Of<ILogger<InMemoryAddressRepository>>())
			, new InMemoryCountryRepository(Mock.Of<ILogger<InMemoryCountryRepository>>()));

		_existingCustomer = new Customer
		{
			FirstName = "Remco",
			LastName = "Lam",
			Street = "Dassenlaan 10",
			City = "Hilversum",
			Postcode = "1216EK",
			Country = "Netherlands"
		};

		await _customerService.StoreAsync(_existingCustomer);
	}


	[TestMethod]
	public async Task GivenHaveACustomer_AndICallAServiceToGetTheCustomer_ThenTheCustomerIsReturned()
	{
		var result = await _customerService.GetByIdentityAsync(_existingCustomer.Id);


		Assert.IsTrue(result.IsSuccess);

		Assert.AreEqual(_existingCustomer.Id, result.Value.Id);
		Assert.AreEqual(_existingCustomer.FirstName, result.Value.FirstName);
		Assert.AreEqual(_existingCustomer.LastName, result.Value.LastName);
		Assert.AreEqual(_existingCustomer.Street, result.Value.Street);
		Assert.AreEqual(_existingCustomer.City, result.Value.City);
		Assert.AreEqual(_existingCustomer.Postcode, result.Value.Postcode);
		Assert.AreEqual(_existingCustomer.Country, result.Value.Country);
	}

	[TestMethod]
	public async Task GivenInsertACustomer_AndICallAServiceToGetTheCustomer_ThenTheCustomerIsIOnSerted_AndTheCustomerIsReturned()
	{
		var customer = new Customer
		{
			FirstName = "Pietje",
			LastName = "Puk",
			Street = "Albrechtlaan 2",
			City = "Amsterdome",
			Postcode = "12345AB",
			Country = "Netherlands"
		};

		await _customerService.StoreAsync(customer);


		var result = await _customerService.GetByIdentityAsync(customer.Id);


		Assert.IsTrue(result.IsSuccess);

		Assert.AreEqual(customer.Id, result.Value.Id);
		Assert.AreEqual(customer.FirstName, result.Value.FirstName);
		Assert.AreEqual(customer.LastName, result.Value.LastName);
		Assert.AreEqual(customer.Street, result.Value.Street);
		Assert.AreEqual(customer.City, result.Value.City);
		Assert.AreEqual(customer.Postcode, result.Value.Postcode);
		Assert.AreEqual(customer.Country, result.Value.Country);
	}
}