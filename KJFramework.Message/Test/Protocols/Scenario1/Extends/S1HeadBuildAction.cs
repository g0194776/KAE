using System;
using KJFramework.Messages.Extends.Actions;

namespace Test.Protocols.Scenario1.Extends
{
    public class S1HeadBuildAction : HeadBuildAction
    {
        #region Overrides of HeadBuildAction

        /// <summary>
        ///     ����һ����Ϣͷ��
        ///     <para>* ��Ϣͷ������涨�� ����ͷ�����ݵ���������С��0��</para>
        /// </summary>
        /// <param name="data">�����ֶ�Ԫ����</param>
        /// <returns>����ͷ��</returns>
        public override byte[] Bind(byte[] data)
        {
            return BitConverter.GetBytes(data.Length);
        }

        /// <summary>
        ///     ��ȡ��Ϣͷ��
        ///     <para>* ��Ϣͷ������涨�� ����ͷ�����ݵ���������С��0��</para>
        /// </summary>
        /// <typeparam name="T">��Ϣͷ������</typeparam>
        /// <param name="data">Ԫ����</param>
        /// <returns>������ȡ������Ϣͷ��</returns>
        public override T Pickup<T>(byte[] data)
        {
            return (T)(Object)BitConverter.ToInt32(data, 0);
        }

        #endregion
    }
}