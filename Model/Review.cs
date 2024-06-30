namespace Pokemon.Model
{
    public class Review
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int Rating { get; set; }

        //One(Pokemon) - Many(Review)
        public Pokemont Pokemon { get; set; }

        //One(Reviewer) - Many(Review)
        public Reviewer Reviewer { get; set; }
    }
}
