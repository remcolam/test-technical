using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rabobank.TechnicalTest.GCOB.Controllers;
using Rabobank.TechnicalTest.GCOB.Services;
using Rabobank.TechnicalTest.GCOB.Services.Contracts;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Tests.Services
{
    [TestClass]
    public class CustomerControllerTest
    {
		private ILogger<CustomerController> _logger;
		private ICustomerService _customerService;

		[TestInitialize]
        public void Initialize()
        {
            _logger = Mock.Of<ILogger<CustomerController>>();

            _customerService = new CustomerService(Mock.Of<ILogger<CustomerService>>()
                , new InMemoryCustomerRepository(Mock.Of<ILogger<InMemoryCustomerRepository>>())
                , new InMemoryAddressRepository(Mock.Of<ILogger<InMemoryAddressRepository>>())
                , new InMemoryCountryRepository(Mock.Of<ILogger<InMemoryCountryRepository>>()));

        }


		[TestMethod]
        public async Task GivenHaveACustomer_AndICallAServiceToGetTheCustomer_ThenTheCustomerIsReturned()
        {
			var customer = new Customer
			{
				FirstName = "Remco",
				LastName = "Lam",
				Street = "Dassenlaan 10",
				City = "Hilversum",
				Postcode = "1216EK",
				Country = "Netherlands"
			};

			await _customerService.StoreAsync(customer);


			var customerController = new CustomerController(_logger, _customerService);
            var response = await customerController.Get(customer.Id);

            Assert.IsInstanceOfType(response, typeof(OkObjectResult));

            var result = ((OkObjectResult)response).Value as Customer;
            Assert.IsNotNull(result);
			Assert.AreEqual(customer.Id, result.Id);
			Assert.AreEqual(customer.FirstName, result.FirstName);
			Assert.AreEqual(customer.LastName, result.LastName);
			Assert.AreEqual(customer.Street, result.Street);
			Assert.AreEqual(customer.City, result.City);
			Assert.AreEqual(customer.Postcode, result.Postcode);
			Assert.AreEqual(customer.Country, result.Country);
		}
	}
}