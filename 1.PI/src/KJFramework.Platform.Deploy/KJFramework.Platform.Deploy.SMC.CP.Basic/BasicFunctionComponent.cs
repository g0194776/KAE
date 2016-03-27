using System;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;
using KJFramework.Net.Channels.Uri;
using KJFramework.Plugin;
using KJFramework.ServiceModel.Bussiness.Default.Services;
using KJFramework.ServiceModel.Elements;

namespace KJFramework.Platform.Deploy.SMC.CP.Basic
{
    /// <summary>
    ///     SMC基础功能组件
    /// </summary>
    public class BasicFunctionComponent : DynamicDomainComponent
    {
        #region Constructor

        /// <summary>
        ///     SMC基础功能组件
        /// </summary>
        public BasicFunctionComponent()
        {
            _name = "SMC基础远程服务功能组件";
            _pluginInfo = new PluginInfomation();
            _pluginInfo.CatalogName = "Plugins";
            _pluginInfo.Version = "0.0.0.1";
            _pluginInfo.ServiceName = "SMC.Components.Basic";
            _pluginInfo.Description = "SMC基础功能组件，提供了服务的基础控制能力";
            //add a rule for get address.
            _ruleTable.Add("GetRpcAddress", args => _address);
        }

        #endregion

        #region Members

        private ServiceHost _controlHost;
        private string _address = "tcp://localhost:9981/ServiceController";

        #endregion

        #region Overrides of DynamicDomainComponent

        protected override void InnerStart()
        {
            _controlHost = new ServiceHost(typeof(ServiceControllerImps), new TcpBinding(new TcpUri(_address)));
            _controlHost.IsSupportExchange = true;
            _controlHost.Opened += Opened;
            _controlHost.Closed += Closed;
            _controlHost.Open();
            Console.WriteLine("Component : #BasicFunctionComponent  started !");
        }

        protected override void InnerStop()
        {
            if (_controlHost != null)
            {
                _controlHost.Opened -= Opened;
                _controlHost.Closed -= Closed;
                _controlHost.Close();
                _controlHost = null;
            }
            Console.WriteLine("Component : #BasicFunctionComponent  stoped !");
        }

        protected override void InnerOnLoading()
        {
            Console.WriteLine("Component : #BasicFunctionComponent  loading......!");
        }

        protected override HealthStatus InnerCheckHealth()
        {
            return _controlHost != null ? HealthStatus.Good : HealthStatus.Death;
        }

        #endregion

        #region Events

        void Closed(object sender, System.EventArgs e)
        {
            Console.WriteLine("#CONNECT. Service : controller host service has been closed !");
        }

        void Opened(object sender, System.EventArgs e)
        {
            Console.WriteLine("#CONNECT. Service : controller host service has been opend !");
        }

        #endregion
    }
}