using System;
using System.Threading;
using KJFramework.Dynamic.Components;
using KJFramework.Enums;
using KJFramework.Timer;

namespace Component2
{
    public class Component2 : DynamicDomainComponent
    {
        #region Members

        private LightTimer _timer;

        #endregion

        #region Overrides of DynamicDomainComponent

        /// <summary>
        ///     开始执行
        /// </summary>
        protected override void InnerStart()
        {
            Console.WriteLine("Component2 activing. #domain: " + AppDomain.CurrentDomain.FriendlyName);
            ThreadPool.QueueUserWorkItem(delegate
                                             {
                                                 Thread.Sleep(5000);
                                                 Console.WriteLine("*******我重生啦~~~~");
                                                 //throw new Exception("Component2 sick...");
                                             });
        }

        /// <summary>
        ///     停止执行
        /// </summary>
        protected override void InnerStop()
        {
            _enable = false;
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
        }

        /// <summary>
        ///     加载后需要做的动作
        /// </summary>
        protected override void InnerOnLoading()
        {
            Console.WriteLine("#Component2 loading......");
        }

        /// <summary>
        ///     检查当前组件的健康状况
        /// </summary>
        /// <returns>返回健康状况</returns>
        protected override HealthStatus InnerCheckHealth()
        {
            return HealthStatus.Good;
        }

        #endregion
    }
}
