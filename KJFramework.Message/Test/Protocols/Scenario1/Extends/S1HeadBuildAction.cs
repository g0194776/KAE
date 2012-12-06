using System;
using KJFramework.Messages.Extends.Actions;

namespace Test.Protocols.Scenario1.Extends
{
    public class S1HeadBuildAction : HeadBuildAction
    {
        #region Overrides of HeadBuildAction

        /// <summary>
        ///     构造一个消息头部
        ///     <para>* 消息头部构造规定： 所有头部数据的索引，均小于0。</para>
        /// </summary>
        /// <param name="data">所有字段元数据</param>
        /// <returns>返回头部</returns>
        public override byte[] Bind(byte[] data)
        {
            return BitConverter.GetBytes(data.Length);
        }

        /// <summary>
        ///     提取消息头部
        ///     <para>* 消息头部构造规定： 所有头部数据的索引，均小于0。</para>
        /// </summary>
        /// <typeparam name="T">消息头部类型</typeparam>
        /// <param name="data">元数据</param>
        /// <returns>返回提取到的消息头部</returns>
        public override T Pickup<T>(byte[] data)
        {
            return (T)(Object)BitConverter.ToInt32(data, 0);
        }

        #endregion
    }
}