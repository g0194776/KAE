namespace KJFramework.Platform.Deploy.CSN.CP.Connector.SubscribeObjs
{
    /// <summary>
    ///     数据库订阅对象元接口，提供了相关的基本操作。
    /// </summary>
    public interface IDBSubscribeObject : ISubscribeObject
    {
        /// <summary>
        ///     订阅数据库
        /// </summary>
        /// <param name="db">数据库名称集合</param>
        void AddSubscribe(params string[] db);
        /// <summary>
        ///     订阅指定DB中的数据表
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <param name="tables">数据表名称集合</param>
        void AddSubscribe(string db, params string[] tables);
        /// <summary>
        ///     订阅指定DB中，指定数据表的相关列
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <param name="table">数据表名称</param>
        /// <param name="column">列名称集合</param>
        void AddSubscribe(string db, string table, params string[] column);
        /// <summary>
        ///     移除对于指定DB的订阅
        /// </summary>
        /// <param name="db">数据库名称集合</param>
        void RemoveSubscribe(params string[] db);
        /// <summary>
        ///     移除针对于指定DB中表的订阅
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <param name="tables">数据表名称集合</param>
        void RemoveSubscribe(string db, params string[] tables);
        /// <summary>
        ///     移除针对于指定DB指定表的字段订阅
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <param name="table">数据表名称集合</param>
        /// <param name="column">列名称集合</param>
        void RemoveSubscribe(string db, string table, params string[] column);
        /// <summary>
        ///     检测当前对象是否订阅了指定数据库
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <returns>返回是否订阅的状态</returns>
        bool CheckIntersting(string db);
        /// <summary>
        ///     检测当前对象是否订阅了指定数据库和这个数据库中的表
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <param name="table">表名</param>
        /// <returns>返回是否订阅的状态</returns>
        bool CheckIntersting(string db, string table);
        /// <summary>
        ///     检测当前对象是否订阅了指定数据库和这个数据库中的表以及相关列
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <param name="table">表名</param>
        /// <param name="column">列名</param>
        /// <returns>返回是否订阅的状态</returns>
        bool CheckIntersting(string db, string table, string column);
    }
}