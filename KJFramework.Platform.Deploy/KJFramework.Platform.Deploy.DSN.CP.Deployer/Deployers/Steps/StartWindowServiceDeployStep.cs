using System;
using System.ServiceProcess;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps
{
    /// <summary>
    ///     ¿ªÆôWINDOWS SERVICE²¿Êð²½Öè
    /// </summary>
    public class StartWindowServiceDeployStep : DeployStep
    {
        protected override bool InnerExecute(out object[] context, params object[] args)
        {
            if (args == null || args.Length != 1)
            {
                throw new ArgumentException("Wrong arguments.");
            }
            string serviceName = (string) args[0];
            _reporter.Notify("#Begin find service: " + serviceName + "......");
            foreach (ServiceController service in ServiceController.GetServices())
            {
                if (service.ServiceName == serviceName)
                {
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        _reporter.Notify("#Service: " + serviceName + "has already started!");
                    }
                    else
                    {
                        service.Start();
                        _reporter.Notify("#Service: " + serviceName + "started!");
                    }
                    context = null;
                    return true;
                }
            }
            _reporter.Notify("#Can not find service: " + serviceName + "!");
            context = null;
            return false;
        }
    }
}