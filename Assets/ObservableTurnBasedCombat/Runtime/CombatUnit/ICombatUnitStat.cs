using System;


namespace ObservableTurnBasedCombat.Application
{
    public interface ICombatUnitStat : IEquatable<ICombatUnitStat>
    {
        public UnitStatId Id { get; }


        bool TryGetById(UnitStatId id, out ICombatUnitStat attribute);
        bool Contains(UnitStatId id);
        bool RemoveById(UnitStatId id);


        bool Contains(ICombatUnitStat stat);
        void Add(ICombatUnitStat stat);

    }
}
