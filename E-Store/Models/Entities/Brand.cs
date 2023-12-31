﻿using System.ComponentModel.DataAnnotations;

namespace E_Store.Models.Entities;

public class Brand: BaseEntity
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }

    [StringLength(400)]
    public string Description { get; set; }

    [StringLength(1000)]
    public string ImageUrl { get; set; }
}
