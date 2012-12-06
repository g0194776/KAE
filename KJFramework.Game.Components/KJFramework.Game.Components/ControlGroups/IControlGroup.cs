using System;
using KJFramework.Game.Components.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.ControlGroups
{
    /// <summary>
    ///     控件组元接口，提供了相关的基本属性结构
    /// </summary>
    public interface IControlGroup<T>
        where T : Control
    {
        /// <summary>
        ///     获取或设置控件组位置
        /// </summary>
        Vector2 Position { get; set; }
        /// <summary>
        ///     获取或设置控件组背景图
        /// </summary>
        Texture2D Background { get; set; }
        /// <summary>
        ///     获取控件组中具有指定名称的控件
        /// </summary>
        /// <typeparam name="T">控件类型</typeparam>
        /// <param name="name">控件名称</param>
        /// <returns>返回具有指定名称的控件</returns>
        T GetControl(string name);
        /// <summary>
        ///     将一个控件加入控件组
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="position">
        ///     被加入控件的显示偏移
        ///     <para>* 此偏移量是相对于控件组偏移的。</para>
        /// </param>
        void AddControl(T control, Vector2 position);
    }
}