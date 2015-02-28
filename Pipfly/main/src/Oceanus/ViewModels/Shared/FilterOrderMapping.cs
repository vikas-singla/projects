using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Oceanus.ViewModels.Shared
{
    public class FilterOrderMapping<T> : Dictionary<string, Expression<Func<T, string>>>
    {
    }
}