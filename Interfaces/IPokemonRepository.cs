using Pokemon.Model;

namespace Pokemon.Interfaces
{
    public interface IPokemonRepository
    {
        //Get All Pokemon
        ICollection<Pokemont> GetAllPokemons();

        //Get Single Pokemon by Id
        Pokemont GetPokemon(int id);

        //Get Single Pokemon by Name
        Pokemont GetPokemon(string name);

        //Get Count Pokemon
        decimal GetPokemonRating(int pokeId);

        // IsExists
        bool PokemonExists(int pokeId);

        //Create
        bool CreatePokemon(int ownerId,int categoryId , Pokemont pokemon);

        //update
        bool UpdatePokemon(int ownerId, int categoryId, Pokemont pokemon);

        //delete
        bool DeletePokemon(Pokemont pokemon);

        //Save
        bool Save();
    }
}
