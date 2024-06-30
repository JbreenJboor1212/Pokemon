using AutoMapper;
using Pokemon.Dto;
using Pokemon.Model;

namespace Pokemon.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemont, PokemonDto>();//Return Data When I Read [Return PokemonDto]
            CreateMap<PokemonDto, Pokemont>();//Return Data When I Create [Return Pokemon]


            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();


            CreateMap<Country,CountryDto>();
            CreateMap<CountryDto, Country>();


            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerDto, Owner>();

            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewDto, Review>();


            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<ReviewerDto, Reviewer>();
        }
    }
}
