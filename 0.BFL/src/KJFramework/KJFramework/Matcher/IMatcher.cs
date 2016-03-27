using System;
using System.Collections.Generic;
using System.Xml;
using KJFramework.Matcher.Rule;

namespace KJFramework.Matcher
{
    /// <summary>
    ///     ƥ����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IMatcher
    {
        /// <summary>
        ///     ����һ���ļ�����ƥ�����
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="path">�ļ�ȫ·��</param>
        /// <param name="rule">ƥ�����</param>
        /// <returns>����ƥ���Ľ��</returns>
        T Match<T>(String path, IMatcherRule rule);
        /// <summary>
        ///     ƥ�����
        /// </summary>
        /// <param name="path">�ļ�ȫ·��</param>
        /// <param name="tag">��������</param>
        /// <param name="rule">ƥ�����</param>
        /// <returns>����ƥ����һ���ֵ伯��</returns>
        Dictionary<String, XmlNode> Match(String path, String tag, IMatcherRule rule);
    }
}