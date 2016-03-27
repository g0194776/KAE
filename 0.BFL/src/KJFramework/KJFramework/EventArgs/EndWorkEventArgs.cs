using System;
namespace KJFramework.EventArgs
{
    public delegate void DelegateEndWork(Object sender, EndWorkEventArgs e);
    /// <summary>
    ///     工作者停止工作事件
    /// </summary>
    public class EndWorkEventArgs : System.EventArgs
    {
    }
}