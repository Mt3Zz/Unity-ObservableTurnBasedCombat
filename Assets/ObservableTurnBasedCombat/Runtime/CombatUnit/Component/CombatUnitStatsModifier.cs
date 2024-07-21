using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnitStatsModifier : BaseCombatUnitComponent
    {
        public UnitStatsId TargetStatId { get; protected set; }


        public CombatUnitStatsModifier(UnitStatsModifierId id, UnitStatsId targetStatId)
            :base(id, UnitComponentType.StatsModefier)
        {
            TargetStatId = targetStatId;
        }
    }
}
