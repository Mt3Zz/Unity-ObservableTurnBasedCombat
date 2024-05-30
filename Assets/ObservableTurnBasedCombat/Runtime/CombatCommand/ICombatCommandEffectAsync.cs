using Cysharp.Threading.Tasks;
using System.Threading;

namespace ObservableTurnBasedCombat.BusinessLogic
{
    public interface ICombatCommandEffectAsync
    {
        CommandEffectId Id {  get; }

        UniTask BeforeExecute(CancellationToken token);
        UniTask Execute(CancellationToken token);
        UniTask Complete(CancellationToken token);
    }
}
