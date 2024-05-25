

namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    using BusinessLogic;


    internal class FakeCombatJob : CombatJobBase
    {
        protected override void OnBeforeExecute() { }
        protected override void OnExecute() { }
        protected override void OnComplete() { }
    }
}
