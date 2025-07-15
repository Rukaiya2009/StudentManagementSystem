using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentManagementSystem.Helpers
{
    public static class EnumHelper
    {
        public static string GetDisplayName(Enum enumValue)
        {
            return enumValue
                .GetType()
                .GetMember(enumValue.ToString())[0]
                .GetCustomAttribute<DisplayAttribute>()?
                .Name ?? enumValue.ToString();
        }

        public static List<SelectListItem> ToSelectList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = GetDisplayName(e as Enum)
            }).ToList();
        }
    }
} 