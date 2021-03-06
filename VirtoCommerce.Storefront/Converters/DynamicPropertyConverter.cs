﻿using System.Linq;
using Newtonsoft.Json.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class DynamicPropertyConverter
    {
        public static DynamicProperty ToWebModel(this CartModule.Client.Model.DynamicObjectProperty dto)
        {
            return dto.JsonConvert<OrderModule.Client.Model.DynamicObjectProperty>().ToWebModel();
        }

        public static DynamicProperty ToWebModel(this CustomerModule.Client.Model.DynamicObjectProperty dto)
        {
            return dto.JsonConvert<OrderModule.Client.Model.DynamicObjectProperty>().ToWebModel();
        }

        public static DynamicProperty ToWebModel(this MarketingModule.Client.Model.DynamicObjectProperty dto)
        {
            return dto.JsonConvert<OrderModule.Client.Model.DynamicObjectProperty>().ToWebModel();
        }

        public static DynamicProperty ToWebModel(this QuoteModule.Client.Model.DynamicObjectProperty dto)
        {
            return dto.JsonConvert<OrderModule.Client.Model.DynamicObjectProperty>().ToWebModel();
        }

        public static DynamicProperty ToWebModel(this StoreModule.Client.Model.DynamicObjectProperty dto)
        {
            return dto.JsonConvert<OrderModule.Client.Model.DynamicObjectProperty>().ToWebModel();
        }

        public static QuoteModule.Client.Model.DynamicObjectProperty ToQuoteApiModel(this DynamicProperty dto)
        {
            return dto.ToCartApiModel().JsonConvert<QuoteModule.Client.Model.DynamicObjectProperty>();
        }

        public static DynamicProperty ToWebModel(this OrderModule.Client.Model.DynamicObjectProperty dto)
        {
            var result = new DynamicProperty();

            result.InjectFrom<NullableAndEnumValueInjecter>(dto);

            if (dto.DisplayNames != null)
            {
                result.DisplayNames = dto.DisplayNames.Select(x => new LocalizedString(new Language(x.Locale), x.Name)).ToList();
            }

            if (dto.Values != null)
            {
                if (result.IsDictionary)
                {
                    var dictValues = dto.Values.Where(x => x.Value != null)
                        .Select(x => x.Value)
                        .Cast<JObject>()
                        .Select(x => x.ToObject<Platform.Client.Model.DynamicPropertyDictionaryItem>())
                        .ToArray();

                    result.DictionaryValues = dictValues.Select(x => x.ToWebModel()).ToList();
                }
                else
                {
                    result.Values = dto.Values.Select(x => x.ToWebModel()).ToList();
                }
            }

            return result;
        }

        public static CartModule.Client.Model.DynamicObjectProperty ToCartApiModel(this DynamicProperty dynamicProperty)
        {
            var result = new CartModule.Client.Model.DynamicObjectProperty();

            result.InjectFrom<NullableAndEnumValueInjecter>(dynamicProperty);

            if (dynamicProperty.Values != null)
            {
                result.Values = dynamicProperty.Values.Select(v => v.ToCartApiModel()).ToList();
            }
            else if (dynamicProperty.DictionaryValues != null)
            {
                result.Values = dynamicProperty.DictionaryValues.Select(x => x.ToCartApiModel()).ToList();
            }

            return result;
        }


        private static DynamicPropertyDictionaryItem ToWebModel(this Platform.Client.Model.DynamicPropertyDictionaryItem dto)
        {
            var result = new DynamicPropertyDictionaryItem();
            result.InjectFrom<NullableAndEnumValueInjecter>(dto);
            if (dto.DisplayNames != null)
            {
                result.DisplayNames = dto.DisplayNames.Select(x => new LocalizedString(new Language(x.Locale), x.Name)).ToList();
            }
            return result;
        }

        private static LocalizedString ToWebModel(this OrderModule.Client.Model.DynamicPropertyObjectValue dto)
        {
            return new LocalizedString(new Language(dto.Locale), dto.Value.ToString());
        }

        private static CartModule.Client.Model.DynamicPropertyObjectValue ToCartApiModel(this DynamicPropertyDictionaryItem dictItem)
        {
            var result = new CartModule.Client.Model.DynamicPropertyObjectValue { Value = dictItem };
            return result;
        }

        private static CartModule.Client.Model.DynamicPropertyObjectValue ToCartApiModel(this LocalizedString dynamicPropertyObjectValue)
        {
            var result = new CartModule.Client.Model.DynamicPropertyObjectValue
            {
                Value = dynamicPropertyObjectValue.Value,
                Locale = dynamicPropertyObjectValue.Language.CultureName
            };

            return result;
        }
    }
}
