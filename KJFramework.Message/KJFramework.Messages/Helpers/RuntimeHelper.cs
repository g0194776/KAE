using System.Collections.Generic;
using KJFramework.Logger;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Objects;

namespace KJFramework.Messages.Helpers
{
    /// <summary>
    ///     运行时帮助器
    /// </summary>
    public sealed class RuntimeHelper
    {
        #region Methods

        /// <summary>
        ///     预算一个运行时对象的完整二进制数据大小
        /// </summary>
        /// <param name="obj">被计算的对象</param>
        /// <returns>
        ///     返回计算后的大小
        ///     <para>* 如果返回-1则表明计算失败</para>
        /// </returns>
        public static int CalcSize(IntellectObject obj)
        {
            if (obj == null) return -1;
            try
            {
                ToBytesAnalyseSet analyseSet = Analyser.ToBytesAnalyser.Analyse(obj);
                if (analyseSet.DynamicAnalyseResults == null) return analyseSet.InitializeSize;
                int totalSize = analyseSet.InitializeSize;
                List<SerializePropertyStub> list = new List<SerializePropertyStub>();
                //insert fixed field
                if (analyseSet.FixedAnalyseResults != null)
                {
                    foreach (ToBytesAnalyseResult analyseResult in analyseSet.FixedAnalyseResults)
                        list.Add(new SerializePropertyStub { AnalyseResult = analyseResult, Value = analyseResult.GetValue(obj) });
                }
                //insert dynamic field
                foreach (ToBytesAnalyseResult analyseResult in analyseSet.DynamicAnalyseResults)
                {
                    object value = analyseResult.GetValue(obj);
                    if (value == null)
                    {
                        if (analyseResult.Attribute.IsRequire) throw new System.Exception(string.Format("Cannot calc rumtime size for target type, because IsRequire = true has been set at this property! details below:\r\nType: {0}\r\nId: {1}\r\n", analyseResult.Property.PropertyType, analyseResult.Attribute.Id));
                        continue;
                    }
                    if (value is string && string.IsNullOrEmpty((string)value)) continue;
                    //id(1) + object size.
                    totalSize += (1 + analyseResult.CalcProcessor(value));
                    list.Add(new SerializePropertyStub { AnalyseResult = analyseResult, Value = value });
                }
                obj.SetSerializableProperty(list);
                obj.RuntimeSize = totalSize;
                return totalSize;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return -1;
            }
        }

        #endregion
    }
}