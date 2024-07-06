
namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    using Application;
    using System.Collections.Generic;

    public class FakeCombatUnitComponent : AbstractCombatUnitComponent
    {
        public FakeCombatUnitComponent
            (
            FakeUnitComponentId id,
            UnitComponentType type,
            IEnumerable<UnitComponentId> links = null
            )
            : base(id, type, links)
        {
            
        }
    }
}
