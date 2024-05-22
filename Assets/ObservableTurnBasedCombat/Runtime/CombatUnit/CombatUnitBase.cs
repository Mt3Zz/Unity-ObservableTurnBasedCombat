using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.BusinessLogic
{
    public class CombatUnitBase
    {
        public UnitAttributes Attributes { get; }
        public UnitStats Stats { get; }

        public UniqueUnitAttributes UniqueAttributes { get; }
        public UniqueUnitStats UniqueStats { get; }

        public StatsModifiers Modifiers { get; }
        public IrremovableStatsModifiers IrremobableModifiers { get; }
    }
}
