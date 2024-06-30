namespace Pokemon.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Many(Pokemon) - Many(Category)
        public ICollection<PokemonCategory> PokemonCategories { get; set; }
    }
}
