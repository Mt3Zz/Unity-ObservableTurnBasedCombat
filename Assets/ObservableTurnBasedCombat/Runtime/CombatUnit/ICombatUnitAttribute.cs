using System;


namespace ObservableTurnBasedCombat.Application
{
    public interface ICombatUnitAttribute : IEquatable<ICombatUnitAttribute>
    {
        public UnitAttributeId Id { get; }


        bool TryGetById(UnitAttributeId id, out ICombatUnitAttribute attribute);
        bool Contains(UnitAttributeId id);


        void Add(ICombatUnitAttribute attribute);
        void Remove(ICombatUnitAttribute attribute);


        //public abstract void Display();


    }
}
