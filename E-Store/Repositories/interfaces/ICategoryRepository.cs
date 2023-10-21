using E_Store.Models.Entities;

namespace E_Store.Repositories.interfaces;

public interface ICategoryRepository
{
    public Task<List<Category>> GetCategoriesImages();
}
