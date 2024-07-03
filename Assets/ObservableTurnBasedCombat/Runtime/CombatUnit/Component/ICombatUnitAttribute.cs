using System;


namespace ObservableTurnBasedCombat.Application
{
    public interface ICombatUnitAttribute : IEquatable<ICombatUnitAttribute>
    {
        public UnitAttributeId Id { get; }


        bool TryGetById(UnitAttributeId id, out ICombatUnitAttribute attribute);
        bool Contains(UnitAttributeId id);
        bool RemoveById(UnitAttributeId id);


        void Add(ICombatUnitAttribute attribute);
        bool Contains(ICombatUnitAttribute attribute);


        //public abstract void Display();


    }
}
