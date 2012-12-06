using KJFramework.Dynamic.Components;

namespace KJFramework.Dynamic.Visitors
{
    public class Visitor
    {
        /// <summary>
        ///     ���ݷ������ƴ���һ���÷���ķ�����
        /// </summary>
        /// <param name="component">���������</param>
        /// <returns>���ط��������</returns>
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