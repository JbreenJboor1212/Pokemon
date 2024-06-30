namespace Pokemon.Model
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //One(Country) - Many(Owner)
        public ICollection<Owner> Owners { get; set; }
    }
}
