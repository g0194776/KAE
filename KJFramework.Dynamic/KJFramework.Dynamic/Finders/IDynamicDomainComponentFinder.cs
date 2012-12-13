using System;
using System.Collections.Generic;
using KJFramework.Dynamic.Structs;

namespace KJFramework.Dynamic.Finders
{
    /// <summary>
    ///     动态程序域组件查找器，提供了相关的基本操作。
    /// </summary>
    public interface IDynamicDomainComponentFinder : IDynamicFinder<List<DomainComponentEntryInfo>>, IDisposable
    {
    }
}