using System.Collections.Generic;

namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    using Application;


    public class FakeCombatCommandAsync : CombatCommandAsync
    {
        public FakeCombatCommandAsync(CommandId id)
            :base
            (
                 new BaseCombatCommandAsync
                 (
                     id,
                     new List<ICombatCommandEffectAsync>
                     {
                        new FakeCombatCommandEffectAsync()
                     }
                 )
            )
        { }
    }
}
