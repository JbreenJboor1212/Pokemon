namespace Pokemon.Model
{
    public class PokemonCategory
    {
        //Many(Pokemon) - Many(Category)
        public int CategoryId { get; set; }
        public int PokemonId { get; set; }
        public Pokemont Pokemon { get; set; }
        public Category Category { get; set; }
    }
}
