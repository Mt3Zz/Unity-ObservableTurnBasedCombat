

namespace ObservableTurnBasedCombat
{
    [System.Serializable]
    public sealed class UnitComponentId : AbstractCombatId
    {
        public UnitComponentId(int id, string name) : base(id, name) { }
    }
}
