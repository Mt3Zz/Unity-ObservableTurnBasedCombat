using System;


namespace ObservableTurnBasedCombat.Application
{
    public interface ICombatUnitStat : IEquatable<ICombatUnitStat>
    {
        public UnitStatId Id { get; }


        bool TryGetById(UnitStatId id, out ICombatUnitStat attribute);


        void Add(ICombatUnitStat stat);
        bool RemoveById(UnitStatId id);


        bool Contains(UnitStatId id);
        bool Contains(ICombatUnitStat stat);

    }

    public interface ICombatUnitStat<T> : ICombatUnitStat
        where T : struct
    {
        public T Value { get; }

        public static T InitialValue { get; }
        public static T Default { get; }

    }
}
