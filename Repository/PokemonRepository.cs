using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Pokemon.Data;
using Pokemon.Interfaces;
using Pokemon.Model;

namespace Pokemon.Repository
{
    public class PokemonRepository:IPokemonRepository
    {
        private readonly DataContext _context;


        public PokemonRepository(DataContext context)
        {
            _context = context;
        }


        public ICollection<Pokemont> GetAllPokemons()
        {
            return _context.Pokemons.OrderBy(p => p.Id).ToList();
        }


        public Pokemont GetPokemon(int id)
        {
           return _context.Pokemons.FirstOrDefault(x => x.Id == id);
        }


        public Pokemont GetPokemon(string name)
        {
            return _context.Pokemons.SingleOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }



        public decimal GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews.Where(x => x.Pokemon.Id == pokeId);

            if (review.Count() <= 0)
                return 0;

            return (decimal)review.Sum(r => r.Rating)/ review.Count();
            
        }


        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemons.Any(x => x.Id == pokeId);
        }


        public bool CreatePokemon(int ownerId, int categoryId, Pokemont pokemon)
        {
            if (pokemon == null)
            {
                throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null");
            }

            var owner = _context.Owners.FirstOrDefault(o => o.Id == ownerId);
            if (owner == null)
            {
                throw new ArgumentException("Owner not found", nameof(ownerId));
            }

            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
            {
                throw new ArgumentException("Category not found", nameof(categoryId));
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var pokemonOwner = new PokemonOwner()
                    {
                        Owner = owner,
                        Pokemon = pokemon
                    };

                    var pokemonCategory = new PokemonCategory()
                    {
                        Category = category,
                        Pokemon = pokemon
                    };

                    _context.Add(pokemonOwner);
                    _context.Add(pokemonCategory);
                    _context.Add(pokemon);

                    Save();

                    transaction.Commit();
                    return true;
                }
                catch (DbUpdateException dbEx)
                {
                    transaction.Rollback();
                    Console.Error.WriteLine(dbEx.InnerException?.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.Error.WriteLine(ex.Message);
                    throw;
                }
            }
        }


        public bool Save()
        {
           var saved = _context.SaveChanges();

            return saved > 0 ? true : false;
        }


        public bool UpdatePokemon(int ownerId, int categoryId, Pokemont pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }

        public bool DeletePokemon(Pokemont pokemon)
        {
            _context.Remove(pokemon);
            return Save();
        }
    }
}
