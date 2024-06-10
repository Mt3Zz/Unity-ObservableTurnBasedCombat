using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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


        public CombatCommandAsync Command { get; private set; }


        public void SetCommand(CombatCommandAsync command)
        {
            if (!command.Metadata.ProgressState.Equals(CommandProgressState.NotStarted))
            {
                throw new ArgumentException("ステートがNotStarted以外のコマンドをセットすることはできません。");
            }

            if (Command == null)
            {
                Command = command;
                return;
            }

            if
            (
                Command.Metadata.ProgressState.Equals(CommandProgressState.BeforeExecuted) ||
                Command.Metadata.ProgressState.Equals(CommandProgressState.Executed)
            )
            {
                throw new InvalidOperationException("実行中のコマンドを上書きすることはできません。");
            }

            Command = command;
        }


        public async UniTask RunAsync(CancellationToken token)
        {
            await DFSUtill(Command, token);
        }
        // 深さ優先探索の再帰関数
        private async UniTask DFSUtill(CombatCommandAsync command, CancellationToken token)
        {
            // 追加コマンドに入るまえにBeforeExecuteを実行
            if (command.Metadata.ProgressState.Equals(CommandProgressState.NotStarted))
            {
                _beforeExecuteSubject.OnNext(command.Metadata);
                await command.BeforeExecute(token);
            }
            else
            {
                UnityEngine.Debug.Log($"実行済みのBeforeExecuteをスキップしました。");
            }


            // 追加コマンドのInterruptionがtrueなら、ここで再帰関数を実行
            if (command.hasAdditionalCommand())
                if (command.Interruption)
                    // 再帰関数を実行
                    if (!command.AdditionalCommand.Metadata.ProgressState.Equals(CommandProgressState.Completed))
                    {
                        await DFSUtill(command.AdditionalCommand, token);
                    }


            // Executeを実行
            if (command.Metadata.ProgressState.Equals(CommandProgressState.BeforeExecuted))
            {
                _executeSubject.OnNext(command.Metadata);
                await command.Execute(token);
            }
            else
            {
                UnityEngine.Debug.Log($"実行済みのExecuteをスキップしました。");
            }


            // 追加コマンドのInterruptionがfalseなら、ここで再帰関数を実行
            if (command.hasAdditionalCommand())
                if (!command.Interruption)
                    // 再帰関数を実行
                    if (!command.AdditionalCommand.Metadata.ProgressState.Equals(CommandProgressState.Completed))
                    {
                        await DFSUtill(command.AdditionalCommand, token);
                    }


            // 追加コマンドから出たあとにCompleteを実行
            if (command.Metadata.ProgressState.Equals(CommandProgressState.Executed))
            {
                _completeSubject.OnNext(command.Metadata);
                await command.Complete(token);
            }
            else
            {
                UnityEngine.Debug.Log($"実行済みのCompleteをスキップしました。");
            }
        }
    }
}
