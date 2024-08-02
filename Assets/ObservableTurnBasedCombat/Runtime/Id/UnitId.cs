

namespace ObservableTurnBasedCombat
{
    [System.Serializable]
    public sealed class UnitId : AbstractCombatId
    {
        public UnitId(int id, string name) : base(id, name) { }
    }
}
