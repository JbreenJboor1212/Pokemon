using Microsoft.EntityFrameworkCore;
using Pokemon.Data;
using Pokemon.Interfaces;
using Pokemon.Model;

namespace Pokemon.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public bool CategoryExists(int id)
        {
           return _context.Categories.Any(x => x.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
           return _context.Categories.OrderBy(x => x.Id).ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.SingleOrDefault(x => x.Id == id);
        }

        public ICollection<Pokemont> GetPokemonByCategory(int categoryId)
        {
            return _context.PokemonCategories.Where(x => x.CategoryId == categoryId)
                .Select(c => c.Pokemon)// Return I Want
                .ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ?true: false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
    }
}
