using BuildingBlocks.Domain;

namespace Orders.Domain;

public sealed class Address : ValueObject
{
    private Address(string street, string city, string country, string zipCode)
    {
        Street = street;
        City = city;
        Country = country;
        ZipCode = zipCode;
    }

    public string Street { get; }
    public string City { get; }
    public string Country { get; }
    public string ZipCode { get; }

    public static Result<Address> Create(string street, string city, string country, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            return Result.Failure<Address>(Error.Validation("Address.StreetEmpty", "Street cannot be empty."));

        if (string.IsNullOrWhiteSpace(city))
            return Result.Failure<Address>(Error.Validation("Address.CityEmpty", "City cannot be empty."));

        if (string.IsNullOrWhiteSpace(country))
            return Result.Failure<Address>(Error.Validation("Address.CountryEmpty", "Country cannot be empty."));

        if (string.IsNullOrWhiteSpace(zipCode))
            return Result.Failure<Address>(Error.Validation("Address.ZipCodeEmpty", "ZipCode cannot be empty."));

        return Result.Success(new Address(street.Trim(), city.Trim(), country.Trim(), zipCode.Trim()));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return Country;
        yield return ZipCode;
    }

    public override string ToString() => $"{Street}, {City}, {Country} ({ZipCode})";
}
