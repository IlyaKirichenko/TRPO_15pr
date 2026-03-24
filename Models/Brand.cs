using System;
using System.Collections.Generic;

namespace TRPO_KI_15pr_ElectronicShop.Models;

public partial class Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
