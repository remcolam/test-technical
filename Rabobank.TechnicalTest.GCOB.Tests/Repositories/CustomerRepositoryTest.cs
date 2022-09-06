using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Services;
using Rabobank.TechnicalTest.GCOB.Services.Contracts;

namespace Rabobank.TechnicalTest.GCOB.Tests.Services;

[TestClass]
public class CustomerRepositoryTest
{
	private ICustomerRepository _customerRepository;
	private CustomerDto _existingCustomer;

	[TestInitialize]
	public async Task Initialize()
	{
		_existingCustomer = new CustomerDto
		{
			FirstName = "Remco",
			LastName = "Lam",
		};

		_customerRepository = new InMemoryCustomerRepository(Mock.Of<ILogger<InMemoryCustomerRepository>>());
		_existingCustomer.Id = (await _customerRepository.InsertAsync(_existingCustomer)).Value;

	}


	[TestMethod]
	public async Task GivenHaveACustomer_AndIGetTheCustomerFromTheDB_ThenTheCustomerIsRetrieved()
	{
		var result = await _customerRepository.GetAsync(_existingCustomer.Id);

		Assert.IsTrue(result.IsSuccess);

		Assert.AreEqual(_existingCustomer.Id, result.Value.Id);
		Assert.AreEqual(_existingCustomer.FirstName, result.Value.FirstName);
		Assert.AreEqual(_existingCustomer.LastName, result.Value.LastName);
		Assert.AreEqual(_existingCustomer.AddressId, result.Value.AddressId);
	}
}
