using System;
using System.Diagnostics;
using System.Reflection;
using KJFramework.Dynamic.Components;
using KJFramework.Enums;
using KJFramework.Timer;

namespace Component1
{
    public class Component1 : DynamicDomainComponent
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
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Module module in assembly.GetModules())
                {
                    Console.WriteLine(module.FullyQualifiedName);
                }
            }
            _timer = LightTimer.NewTimer(5000, -1).Start(delegate
                                                             {
                                                                 Console.WriteLine("~Component1 activing. #domain: " + AppDomain.CurrentDomain.FriendlyName);
                                                             }, null);
            _enable = true;
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
            Console.WriteLine("#Component1 loading......");
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
