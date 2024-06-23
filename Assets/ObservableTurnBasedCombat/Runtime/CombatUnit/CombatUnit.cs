using ObservableCollections;
using System.Collections.Generic;


namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnit
    {
        public UnitId Id { get => Metadata.Id; }
        public UnitMetadata Metadata { get; protected set; }


        public ICombatUnitAttribute Attributes { get; protected set; }
        public ICombatUnitStat Stats { get; protected set; }


        public List<CombatUnitStatsModifier> StatsModifiers { get; protected set; }


        public CombatUnitComponentGraph Graph { get; protected set; }
        public ObservableDictionary<UnitComponentId, ICombatUnitComponent> ComponentById { get; protected set; }
    }
}
