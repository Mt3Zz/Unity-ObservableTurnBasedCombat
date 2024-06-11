using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnit
    {
        public UnitId Id { get => Metadata.Id; }
        public UnitMetadata Metadata { get; protected set; }


        public List<ICombatUnitAttribute> Attributes { get; protected set; }
        public List<ICombatUnitStats> Stats { get; protected set; }


        public List<CombatUnitStatsModifier> StatsModifiers { get; protected set; }



    }
}
