
using System;

namespace ObservableTurnBasedCombat
{
    [Serializable]
    public abstract class UnitComponentId : AbstractCombatId, IComponentNode
    {
        public UnitComponentId(int id, string name) : base(id, name) { }


        public bool Equals(IComponentNode other)
        {
            if (other == null || GetType() != other.GetType()) return false;
            return GetHashCode() == other.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as UnitComponentId);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
