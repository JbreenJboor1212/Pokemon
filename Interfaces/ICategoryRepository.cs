using Pokemon.Model;

namespace Pokemon.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int Id);

        ICollection<Pokemont> GetPokemonByCategory(int categoryId);

        bool CategoryExists(int Id);

        bool CreateCategory(Category category);

        bool UpdateCategory(Category category);

        bool DeleteCategory(Category category);

        bool Save();

    }
}
