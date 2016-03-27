using System;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;
using KJFramework.IO.Helper;
using KJFramework.Plugin;
using KJFramework.Timer;

namespace TestComponent1
{
    public class TestComponent1 : DynamicDomainComponent
    {
        public TestComponent1()
        {
            _name = "Test Component 1";
            _pluginInfo = new PluginInfomation();
            _pluginInfo.CatalogName = "Components";
            _pluginInfo.Version = "0.0.0.2";
            _pluginInfo.ServiceName = "Test.Components.1";
            _pluginInfo.Description = "测试组件1的说明";
        }

        private LightTimer _timer;

        #region Overrides of DynamicDomainComponent

        protected override void InnerStart()
        {
            //throw new Exception("这是一个更新时候的错误~");
            if (_timer == null)
            {
                _timer = LightTimer.NewTimer(5000, -1);
                _timer.Start(delegate { ConsoleHelper.PrintLine("PRINT: I am test component 1~~ Kevin.Jee !"); }, null);
            }
            Console.WriteLine("Component : #TestComponent1  started......!");
        }

        protected override void InnerStop()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
            Console.WriteLine("Component : #TestComponent1  stoped......!");
        }

        protected override void InnerOnLoading()
        {
            Console.WriteLine("Component : #TestComponent1  loading......!");
        }

        protected override HealthStatus InnerCheckHealth()
        {
            return _timer == null ? HealthStatus.Death : HealthStatus.Good;
        }

        #endregion
    }
}
