using AutoMapper;
using E_Store.Models.Entities;
using E_Store.ViewModels;

namespace E_Store.Helpers;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductVM>()
            .ForMember(des => des.CategoryName, options => options.MapFrom(src => src.Category.Name))
            .ForMember(des => des.BrandName, options => options.MapFrom(src => src.Brand.Name))
            .ReverseMap();

        CreateMap<Product, ProductEditVM>().ReverseMap();
    }
}
