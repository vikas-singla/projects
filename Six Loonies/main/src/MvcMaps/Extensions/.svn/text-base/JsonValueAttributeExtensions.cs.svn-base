/*
* MvcMaps Preview 1 - A Unified Mapping API for ASP.NET MVC
* Copyright (c) 2009 Chris Pietschmann
* http://mvcmaps.codeplex.com
* Licensed under the Microsoft Reciprocal License (Ms-RL)
* http://mvcmaps.codeplex.com/license
*/

using System;

namespace MvcMaps.Extensions
{
    internal static class JsonValueAttributeExtensions
    {
        internal static string ToJsonValue(this Enum enumValue)
        {
            var attributes = (JsonValueAttribute[])enumValue.GetType().GetField(enumValue.ToString()).GetCustomAttributes(typeof(JsonValueAttribute), false);
            return (attributes.Length > 0 ? attributes[0].JsonValue : string.Empty);
        }
    
    }
}
