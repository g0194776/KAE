using System;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;

namespace KJFramework.Platform.Deploy.DSC.CP.Basic
{
    public class BasicComponent : DynamicDomainComponent
    {
        #region Overrides of DynamicDomainComponent

        protected override void InnerStart()
        {
        }

        protected override void InnerStop()
        {
        }

        protected override void InnerOnLoading()
        {
            Console.WriteLine("Component : #BasicComponent loading......!");
        }

        protected override HealthStatus InnerCheckHealth()
        {
            return HealthStatus.Death;
        }

        #endregion
    }
}
