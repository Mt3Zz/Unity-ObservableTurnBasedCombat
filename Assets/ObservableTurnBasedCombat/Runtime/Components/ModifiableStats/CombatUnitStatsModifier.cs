using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnitStatsModifier : ObservableUnitComponent
    {
        public UnitComponentId TargetStatId { get; protected set; }


        public CombatUnitStatsModifier(UnitComponentId id, UnitComponentId targetStatId)
            :base(id)
        {
            TargetStatId = targetStatId;
        }
    }
}
