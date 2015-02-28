﻿/*
* MvcMaps Preview 1 - A Unified Mapping API for ASP.NET MVC
* Copyright (c) 2009 Chris Pietschmann
* http://mvcmaps.codeplex.com
* Licensed under the Microsoft Reciprocal License (Ms-RL)
* http://mvcmaps.codeplex.com/license
*/

namespace MvcMaps
{
    public interface IMap
    {
        double? CenterLatitude { get; set; }
        double? CenterLongitude { get; set; }
        int? ZoomLevel { get; set; }

        void Render();
    }
}
