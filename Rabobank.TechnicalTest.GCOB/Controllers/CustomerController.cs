using Microsoft.AspNetCore.Mvc;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Services.Contracts;

namespace Rabobank.TechnicalTest.GCOB.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
	private readonly ILogger<CustomerController> _logger;
	private readonly ICustomerService _customerService;

	public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
	{
		_logger = logger;
		_customerService = customerService;
	}

	[HttpGet("{identity}")]
	[ProducesResponseType(typeof(Customer), 200)]
	public async Task<IActionResult> Get(int identity)
	{
		var result = await _customerService.GetByIdentityAsync(identity);

		return result.IsSuccess
			? this.Ok(result.Value)
			: this.NotFound(result.Error);
	}

	[HttpPost]
	[ProducesResponseType(typeof(Customer), 200)]
	public async Task<IActionResult> Post(Customer customer)
	{
		var result = await _customerService.StoreAsync(customer);

		return result.IsSuccess
			? this.Ok(result.Value)
			: this.NotFound(result.Error);
	}
}
