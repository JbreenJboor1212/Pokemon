using Microsoft.EntityFrameworkCore;
using Pokemon.Data;
using Pokemon.Interfaces;
using Pokemon.Model;

namespace Pokemon.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;



        public ReviewRepository(DataContext context)
        {
            _context = context;
        }



        public Review GetReview(int reviewId)
        {
            return _context.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }



        public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
        {
            return _context.Reviews.Where(r => r.Pokemon.Id == pokeId).ToList();
        }



        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }



        public bool ReviewExists(int reviewId)
        {
            return _context.Reviews.Any(r => r.Id == reviewId);
        }



        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }



        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return Save();
        }



        public bool Save()
        {
           var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return Save();
        }

        //Return All Review To Own One Pokemon REmoved
        public bool DeleteReviews(ICollection<Review> reviews)
        {
            _context.Reviews.RemoveRange(reviews);
            return Save();
        }
    }
}
