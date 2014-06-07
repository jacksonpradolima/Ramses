using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Objects.DataClasses;

namespace Ramses.Business
{
    /// <summary>
    /// Defines a type that exposes a predicate to filter items in an collection of elements of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">A EntityObject type implementation</typeparam>
    public interface IEntityFilter<T> where T : EntityObject
    {
        Func<T, bool> Predicate { get; set; }
    }
}
