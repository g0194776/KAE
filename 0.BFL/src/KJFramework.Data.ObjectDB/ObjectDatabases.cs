using System;
using System.IO;

namespace KJFramework.Data.ObjectDB
{
    /// <summary>
    ///     �������ݿ⾲̬��
    /// </summary>
    public class ObjectDatabases
    {
        #region Methods

        /// <summary>
        ///     ��һ���Ѵ��ڵĶ������ݿ��ļ�
        ///     <para>* ���Ŀ���ļ������ڣ������Զ�����һ�����ļ�</para>
        /// </summary>
        /// <param name="filename">�������ݿ�ȫ·����ַ</param>
        /// <returns>���ض������ݿ�ʵ��</returns>
        /// <exception cref="System.ArgumentNullException">��������Ϊ��</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">Ŀ¼������</exception>
        public static IObjectDatabase Open(string filename)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException("filename");
            if(!Directory.Exists(Path.GetFullPath(filename))) throw new DirectoryNotFoundException(Path.GetFullPath(filename));
            return new LocalObjectDatabase(filename);
        }

        #endregion
    }
}