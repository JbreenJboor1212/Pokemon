namespace Pokemon.Model
{
    public class Reviewer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        //One(Reviewer) - Many(Review)
        public ICollection<Review> Reviews { get; set; }
    }
}
