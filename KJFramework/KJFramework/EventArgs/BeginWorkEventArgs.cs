using System;
namespace KJFramework.EventArgs
{
    public delegate void DelegateBeginWork(Object sender, BeginWorkEventArgs e);
    /// <summary>
    ///     工作者开始工作事件
    /// </summary>
    public class BeginWorkEventArgs : System.EventArgs
    {
    }
}