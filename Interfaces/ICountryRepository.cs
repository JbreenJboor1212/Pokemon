using Pokemon.Model;

namespace Pokemon.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();

        Country GetCountry(int countryId);

        Country GetCountryByOwner(int ownerId);

        ICollection<Owner> GetOwnersFromCountry(int countryId);

        bool CountryExist(int countryId);

        bool CreateCountry(Country country);

        bool UpdateCountry(Country country);

        bool DeleteCountry(Country country);

        bool Save();
    }
}
