using E_Store.Models.Entities;

namespace E_Store.Repositories.interfaces;

public interface IBrandRepository
{
    public Task<List<Brand>> GetBrandsImages();
}
