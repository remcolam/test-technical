using Rabobank.TechnicalTest.GCOB.Services;
using Rabobank.TechnicalTest.GCOB.Services.Contracts;

namespace Rabobank.TechnicalTest.GCOB.ServiceExtensions;

public static class ServiceExtensions
{
	public static IServiceCollection AddTechnicalTest(this IServiceCollection services)
	{
		services.AddScoped<ICustomerService, CustomerService>();

		services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();
		services.AddSingleton<IAddressRepository, InMemoryAddressRepository>();
		services.AddSingleton<ICountryRepository, InMemoryCountryRepository>();

		return services;
	}
}
