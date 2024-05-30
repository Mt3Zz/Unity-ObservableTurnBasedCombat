using Cysharp.Threading.Tasks;
using System.Threading;

namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    using System.Diagnostics;
    using BusinessLogic;


    public class FakeCombatCommandEffectAsync : ICombatCommandEffectAsync
    {
        public CommandEffectId Id { get; }

        public async UniTask BeforeExecute(CancellationToken token)
        {
            // Do something before executing the command
            await UniTask.DelayFrame(1);
        }

        public async UniTask Execute(CancellationToken token)
        {
            // Execute the command
            await UniTask.DelayFrame(1);
        }

        public async UniTask Complete(CancellationToken token)
        {
            // Do something after completing the command
            await UniTask.DelayFrame(1);
        }
    }
}
