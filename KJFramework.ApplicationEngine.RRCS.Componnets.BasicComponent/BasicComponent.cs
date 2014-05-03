using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;

namespace KJFramework.ApplicationEngine.RRCS.Componnets.BasicComponent
{
    public class BasicComponent : DynamicDomainComponent
    {
        protected override void InnerStart()
        {
        }

        protected override void InnerStop()
        {
        }

        protected override void InnerOnLoading()
        {
        }

        protected override HealthStatus InnerCheckHealth()
        {
            return HealthStatus.Good;
        }
    }
}
