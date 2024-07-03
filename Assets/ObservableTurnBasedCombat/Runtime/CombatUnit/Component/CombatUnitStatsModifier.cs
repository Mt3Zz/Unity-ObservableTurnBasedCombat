using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnitStatsModifier
    {
        public UnitId Owner { get; private set; }

        public List<UnitStatId> TargetStatIds { get; protected set; }
    }
}
