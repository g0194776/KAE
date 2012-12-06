using KJFramework.Dynamic.Components;

namespace KJFramework.Dynamic.Visitors
{
    public class Visitor
    {
        /// <summary>
        ///     根据服务名称创建一个该服务的访问器
        /// </summary>
        /// <param name="component">程序域组件</param>
        /// <returns>返回服务访问器</returns>
        public static IDynamicObjectVisitor Create(DynamicDomainComponent component)
        {
            if (component == null)
            {
                return null;
            }
            return new DynamicObjectVisitor(component);
        }
    }
}