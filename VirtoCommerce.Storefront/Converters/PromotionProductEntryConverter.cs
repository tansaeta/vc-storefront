﻿using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Converters
{
    public static class PromotionProductEntryConverter
    {
        public static MarketingModule.Client.Model.ProductPromoEntry ToServiceModel(this PromotionProductEntry webModel)
        {
            var serviceModel = new MarketingModule.Client.Model.ProductPromoEntry();

            serviceModel.InjectFrom<NullableAndEnumValueInjecter>(webModel);

            serviceModel.Discount = webModel.Discount != null ? (double?)webModel.Discount.Amount : null;
            serviceModel.Price = webModel.Price != null ? (double?)webModel.Price.Amount : null;
            serviceModel.Variations = webModel.Variations != null ? webModel.Variations.Select(v => v.ToServiceModel()).ToList() : null;

            return serviceModel;
        }
    }
}
