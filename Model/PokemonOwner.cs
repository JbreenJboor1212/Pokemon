namespace Pokemon.Model
{
    public class PokemonOwner
    {
        //Many(Pokemon) - Many(Owner)
        public int PokemonId { get; set; }
        public Pokemont Pokemon { get; set; }

        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
    }
}
