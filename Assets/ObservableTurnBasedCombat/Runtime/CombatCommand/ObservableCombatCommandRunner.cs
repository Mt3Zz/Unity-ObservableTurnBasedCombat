using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;

namespace ObservableTurnBasedCombat.Application
{
    public class ObservableCombatCommandRunner : IDisposable
    {
        /// <summary>
        /// 戦闘コマンドの各メソッドのObservableを返します。
        /// </summary>
        public (
            Observable<CommandMetadata> BeforeExecute,
            Observable<CommandMetadata> Execute,
            Observable<CommandMetadata> Complete
        ) ObservableEvents =>
        (
            _beforeExecuteSubject,
            _executeSubject,
            _completeSubject
        );
        private readonly Subject<CommandMetadata> _beforeExecuteSubject = new Subject<CommandMetadata>();
        private readonly Subject<CommandMetadata> _executeSubject = new Subject<CommandMetadata>();
        private readonly Subject<CommandMetadata> _completeSubject = new Subject<CommandMetadata>();

        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        public void Dispose()
        {
            _beforeExecuteSubject.Dispose();
            _executeSubject.Dispose();
            _completeSubject.Dispose();
        }

        /*
        private enum RunnerState
        {
            NotStarted,
            Running,
            Pause,
            Completed
        }
        private RunnerState _runnerState = RunnerState.NotStarted;
        public bool Running
        {
            get 
            { 
                return _runnerState == RunnerState.Running;
            }
            set
            {
                switch (_runnerState)
                {
                    case RunnerState.NotStarted:
                        break;
                    case RunnerState.Running:
                        if (!value)
                        {
                            _utcs = new UniTaskCompletionSource();
                            _runnerState = RunnerState.Pause;
                        }
                        break;
                    case RunnerState.Pause:
                        if (value)
                        {
                            _utcs.TrySetResult();
                            _runnerState = RunnerState.Running;
                        }
                        break;
                    case RunnerState.Completed:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        private UniTaskCompletionSource _utcs = new UniTaskCompletionSource();
        */

        public CombatCommandAsync Command { get; private set; }


        public bool Initialize(CombatCommandAsync command)
        {
            /*
            if (_runnerState == RunnerState.NotStarted || _runnerState == RunnerState.Completed)
            {
                Command = command;
                return true;
            }
            return false;
            */
            Command = command;
            return true;
        }


        public async UniTask RunAsync(CancellationToken token)
        {
            /*
            if (_runnerState != RunnerState.NotStarted) return;


            _runnerState = RunnerState.Running;
            _utcs.TrySetResult();
            */

            var visted = new HashSet<int>();
            await DFSUtill(Command, visted, token);


            //_runnerState = RunnerState.Completed;
        }
        // 深さ優先探索の再帰関数
        private async UniTask DFSUtill(CombatCommandAsync command, HashSet<int> visited, CancellationToken token)
        {
            // 訪問済みを設定
            visited.Add(command.GetHashCode());


            // Nodeに入る前の処理
            // ChildNodeに入るまえにBeforeExecuteを実行
            _beforeExecuteSubject.OnNext(command.Metadata);
            await command.BeforeExecute(token);


            // RunnerStateに応じて処理を待機
            // await _utcs.Task;


            // ContinuousCommandのInterruptionがtrueなら、ここで再帰関数を実行
            if (command.hasAdditionalCommand())
                if (command.Interruption)
                    // 再帰関数を実行
                    if (!visited.Contains(command.AdditionalCommand.GetHashCode()))
                        await DFSUtill(command.AdditionalCommand, visited, token);


            // Executeを実行
            _executeSubject.OnNext(command.Metadata);
            await command.Execute(token);


            // ContinuousCommandのInterruptionがfalseなら、ここで再帰関数を実行
            if (command.hasAdditionalCommand())
                if (!command.Interruption)
                    // 再帰関数を実行
                    if (!visited.Contains(command.AdditionalCommand.GetHashCode()))
                        await DFSUtill(command.AdditionalCommand, visited, token);


            // ChildNodeから出たあとにCompleteを実行
            _completeSubject.OnNext(command.Metadata);
            await command.Complete(token);
        }
    }
}
