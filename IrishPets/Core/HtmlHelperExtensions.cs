using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;

namespace IrishPets.Core
{
    public static class HtmlHelperExtensions
    {

        public static string DisplayName(this Enum value)
        {
            Type __enumType = value.GetType();
            var __enumValue = Enum.GetName(__enumType, value);
            MemberInfo __member = __enumType.GetMember(__enumValue)[0];

            var __attrs = __member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var __outString = ((DisplayAttribute)__attrs[0]).Name;

            if (null != ((DisplayAttribute)__attrs[0]).ResourceType)
            {
                __outString = ((DisplayAttribute)__attrs[0]).GetName();
            }

            return __outString;
        }

        public static ImgTag ImgTag(this HtmlHelper htmlHelper, string imagePath, string altText) =>
            new ImgTag(imagePath, altText, new UrlHelper(htmlHelper.ViewContext.RequestContext).Content);
    }
}