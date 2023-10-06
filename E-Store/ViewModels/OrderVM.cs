
using E_Store.Data;
using E_Store.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_Store.ViewModels;

public class OrderVM
{
    public string Id { get; set; }

    [Required, StringLength(50)]
    [Display(Name ="First Name")]
    public string FirstName { get; set; }

    [Required, StringLength(50)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [EmailAddress, StringLength(255)]
    public string Email { get; set; }

    [Required, StringLength(30)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Required, StringLength(255)]
    [Display(Name = "Address 1")]
    public string AddressLine1 { get; set; }

    [StringLength(255)]
    [Display(Name = "Address 2", Prompt = "Apartment or suite")]
    public string AddressLine2 { get; set; }

    [Required, StringLength(50)]
    public string City { get; set; }

    [StringLength(50)]
    public string State { get; set; }

    [Required, StringLength(50)]
    public string Country { get; set; }

    [StringLength(20)]
    [Display(Name = "Zip Code")]
    public string ZipCode { get; set; }

    public decimal TotalPrice { get; set; }

    public List<CartItemVM> CartItems { get; set; }
}
