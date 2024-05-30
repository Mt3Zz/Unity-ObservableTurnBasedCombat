using System.Collections.Generic;


namespace ObservableTurnBasedCombat
{
    public sealed class CommandId : AbstractCombatId
    {
        public CommandId(int id, string name) : base(id, name) { }
    }
}
