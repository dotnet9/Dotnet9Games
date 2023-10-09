using System.ComponentModel;
using System.Reflection;

namespace Dotnet9Games.Helpers;

internal static class EnumHelper
{
    /// <summary>
    ///     获取枚举Description值
    /// </summary>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    internal static string Description(this object enumValue)
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        var attribute = field.GetCustomAttribute(typeof(DescriptionAttribute));
        var descriptionAttribute = (DescriptionAttribute)attribute;
        return descriptionAttribute.Description;
    }
}