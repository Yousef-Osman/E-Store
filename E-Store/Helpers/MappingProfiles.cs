using AutoMapper;
using E_Store.Models.Entities;
using E_Store.ViewModels;

namespace E_Store.Helpers;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductVM>()
            .ForMember(des => des.CategoryNames, options => options.MapFrom(src => 
                        src.Categories.Select(a=>a.Category.Name).ToList()))
            .ForMember(des => des.BrandName, options => options.MapFrom(src => src.Brand.Name));

        CreateMap<Product, ProductEditVM>()
            .ForMember(des => des.SelectedBrand, options => options.MapFrom(src => src.Brand.Id))
            .ForMember(des => des.SelectedCategories, options => options.MapFrom(src =>
                        src.Categories.Select(a=>a.Category.Id).ToList()));
    }
}
