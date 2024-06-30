namespace Pokemon.Model
{
    public class Owner
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gym { get; set; }

        //Many(Owner) - One(Country)
        public Country Country { get; set; }

        //Many(Pokemon) - Many(Owner)
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
    }
}
