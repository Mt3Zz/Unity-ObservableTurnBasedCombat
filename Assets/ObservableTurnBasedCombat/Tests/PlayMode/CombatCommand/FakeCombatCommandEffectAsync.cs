using Cysharp.Threading.Tasks;
using System.Threading;

namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    using Application;
    using System.Runtime.InteropServices;

    public class FakeCombatCommandEffectAsync : ICombatCommandEffectAsync
    {
        public CommandEffectId Id { get; }

        public FakeCombatCommandEffectAsync()
        {
            var id = new CommandEffectId(1, "Test");
            Id = id;
        }

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
