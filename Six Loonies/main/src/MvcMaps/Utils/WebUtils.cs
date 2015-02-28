/*
* MvcMaps Preview 1 - A Unified Mapping API for ASP.NET MVC
* Copyright (c) 2009 Chris Pietschmann
* http://mvcmaps.codeplex.com
* Licensed under the Microsoft Reciprocal License (Ms-RL)
* http://mvcmaps.codeplex.com/license
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MvcMaps.Utils
{
    public static class WebUtils
    {
        private static MethodInfo _getWebResourceUrlMethod;
        private static object _getWebResourceUrlLock = new object();

        public static string GetWebResourceUrl<T>(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException("resourceName");
            }

            if (_getWebResourceUrlMethod == null)
            {
                lock (_getWebResourceUrlLock)
                {
                    if (_getWebResourceUrlMethod == null)
                    {
                        _getWebResourceUrlMethod = typeof(System.Web.Handlers.AssemblyResourceLoader).GetMethod(
                            "GetWebResourceUrlInternal",
                            BindingFlags.NonPublic | BindingFlags.Static);
                    }
                }
            }

            return (string)_getWebResourceUrlMethod.Invoke(null,
                new object[] { Assembly.GetAssembly(typeof(T)), resourceName, false, false, null });
        }
    }
}
