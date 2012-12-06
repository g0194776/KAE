using System;
using KJFramework.Game.Components.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.ControlGroups
{
    /// <summary>
    ///     �ؼ���Ԫ�ӿڣ��ṩ����صĻ������Խṹ
    /// </summary>
    public interface IControlGroup<T>
        where T : Control
    {
        /// <summary>
        ///     ��ȡ�����ÿؼ���λ��
        /// </summary>
        Vector2 Position { get; set; }
        /// <summary>
        ///     ��ȡ�����ÿؼ��鱳��ͼ
        /// </summary>
        Texture2D Background { get; set; }
        /// <summary>
        ///     ��ȡ�ؼ����о���ָ�����ƵĿؼ�
        /// </summary>
        /// <typeparam name="T">�ؼ�����</typeparam>
        /// <param name="name">�ؼ�����</param>
        /// <returns>���ؾ���ָ�����ƵĿؼ�</returns>
        T GetControl(string name);
        /// <summary>
        ///     ��һ���ؼ�����ؼ���
        /// </summary>
        /// <param name="control">�ؼ�</param>
        /// <param name="position">
        ///     ������ؼ�����ʾƫ��
        ///     <para>* ��ƫ����������ڿؼ���ƫ�Ƶġ�</para>
        /// </param>
        void AddControl(T control, Vector2 position);
    }
}