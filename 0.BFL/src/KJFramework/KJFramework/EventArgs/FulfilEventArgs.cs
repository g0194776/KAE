﻿using System;

namespace KJFramework.EventArgs
{
    public delegate void DelegateFulfil(Object sender, FulfilEventArgs e);
    /// <summary>
    ///        条件满足触发事件
    /// </summary>
    public class FulfilEventArgs : System.EventArgs
    {
       
    }
}
