using AutoMapper;
using E_Store.Models.Entities;
using E_Store.ViewModels;

namespace E_Store.Helpers;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductVM>()
            .ForMember(des => des.BrandName, options => options.MapFrom(src => src.Brand.Name))
            .ForMember(des => des.CategoryNames, options => options.MapFrom(src =>
                        src.Categories.Select(a => a.Category.Name).ToList()));  //you can remove .ToList()

        CreateMap<Product, ProductEditVM>()
            .ForMember(des => des.SelectedBrand, options => options.MapFrom(src => src.Brand.Id))
            .ForMember(des => des.SelectedCategories, options => options.MapFrom(src =>
                        src.Categories.Select(a => a.CategoryId)))
            .ForMember(des => des.Categories, options => options.Ignore())
            .ForMember(des => des.Brands, options => options.Ignore());

        CreateMap<Category, CategoryVM>();
    }
}
