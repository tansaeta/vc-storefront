﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class CategoryAssociation : Association
    {
        public string CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
