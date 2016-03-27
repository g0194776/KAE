namespace KJFramework.ServiceModel.Bussiness.VSPlugin.Generators
{
    /// <summary>
    ///     源代码文件生成器元接口，提供了相关的基本操作
    /// </summary>
    public interface ISourceFileGenerator
    {
        /// <summary>
        ///     创建一个字段
        /// </summary>
        void GenerateProperty(string type, string name, int id, bool isRequire);
        /// <summary>
        ///     生成文件
        /// </summary>
        void Generate();
    }
}