using System;
using System.Collections.Generic;
using KJFramework.Dynamic.Structs;

namespace KJFramework.Dynamic.Finders
{
    /// <summary>
    ///     ��̬������������������ṩ����صĻ���������
    /// </summary>
    public interface IDynamicDomainComponentFinder : IDynamicFinder<List<DomainComponentEntryInfo>>, IDisposable
    {
    }
}