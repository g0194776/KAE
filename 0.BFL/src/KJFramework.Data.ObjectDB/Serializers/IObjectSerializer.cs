namespace KJFramework.Data.ObjectDB.Serializers
{
    /// <summary>
    ///     对象序列化器接口
    /// </summary>
    public interface IObjectSerializer
    {
        /// <summary>
        ///     将一个对象序列化为二进制表现形式
        /// </summary>
        /// <param name="obj">要被序列化的对象</param>
        /// <returns>返回序列化后的二进制数据</returns>
        byte[] Serialize(object obj);
    }
}