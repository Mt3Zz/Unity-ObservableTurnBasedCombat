

namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    using BusinessLogic;


    internal class FakeCombatCommandAsync : BaseCombatCommandAsync
    {
        protected override void OnBeforeExecute() { }
        protected override void OnExecute() { }
        protected override void OnComplete() { }
    }
}
