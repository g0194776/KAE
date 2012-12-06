using System;
using System.Collections.Generic;

namespace KJFramework.Matcher.Rule
{
    /// <summary>
    ///     ƥ����ƥ�����Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IMatcherRule : IEnumerable<String>
    {
        /// <summary>
        ///     ƥ����
        /// </summary>
        /// <param name="arg">Ҫƥ�������</param>
        /// <returns>����ƥ����</returns>
        bool Check(String arg);
        /// <summary>
        ///     ��ȡƥ���������
        /// </summary>
        int Count { get; }
    }
}