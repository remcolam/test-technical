using Rabobank.TechnicalTest.GCOB.Dtos;

namespace Rabobank.TechnicalTest.GCOB;

public class Customer
{
	public int Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string FullName => $"{this.FirstName} {this.LastName}".Trim();
	public string Street { get; set; }
	public string City { get; set; }
	public string Postcode { get; set; }
	public string Country { get; set; }

	internal CustomerDto CopyTo(CustomerDto customer)
	{
		customer.Id = this.Id;
		customer.FirstName = this.FirstName;
		customer.LastName = this.LastName;
		return customer;
	}

	internal AddressDto CopyTo(AddressDto address)
	{
		address.Street = this.Street;
		address.City = this.City;
		address.Postcode = this.Postcode;

		return address;
	}

	internal static Customer Map(CustomerDto customer, AddressDto address, CountryDto country)
		=> new Customer
		{
			Id = customer.Id,
			FirstName = customer.FirstName,
			LastName = customer.LastName,
			Street = address.Street,
			City = address.City,
			Postcode = address.Postcode,
			Country = country.Name,
		};
}