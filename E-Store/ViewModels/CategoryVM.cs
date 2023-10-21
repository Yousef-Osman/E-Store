﻿using System.ComponentModel.DataAnnotations;

namespace E_Store.ViewModels;

public class CategoryVM
{
    [Required, StringLength(100)]
    public string Name { get; set; }

    [StringLength(1000)]
    public string ImageUrl { get; set; }
}
