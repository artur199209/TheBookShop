using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace TheBookShop.Infrastructure
{
    public static class EnumExtensions
    {
        public static string GetDescription(this object value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description
                ?? value.ToString();
        }

        public static IEnumerable<SelectListItem> GetEnumSelectList<T>()
        {
            return (Enum
                .GetNames(typeof(T))
                .Select(i => new SelectListItem
                {
                    Text = GetDescription(Enum.Parse(typeof(T), i.ToString())),
                    Value = Enum.Parse(typeof(T), i.ToString()).ToString()
                }));
        }
    }
}