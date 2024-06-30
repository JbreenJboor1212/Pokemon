namespace Pokemon.Model
{
    public class Pokemont
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        //One(Pokemon) - Many(Review)
        public ICollection<Review> Reviews { get; set; }

        //Many(Pokemon) - Many(Owner)
        public ICollection<PokemonOwner> PokemonOwners { get; set; }

        //Many(Pokemon) - Many(Category)
        public ICollection<PokemonCategory> PokemonCategories { get; set; }
    }
}
