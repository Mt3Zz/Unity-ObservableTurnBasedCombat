using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading.Tasks;

namespace ObservableTurnBasedCombat.Application
{
    /// <summary>
    /// オブザーバブルな戦闘コマンドランナー。戦闘コマンドの実行を管理し、イベントを提供します。
    /// </summary>
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

        /// <summary>
        /// 現在設定されている戦闘コマンド。
        /// </summary>
        public CombatCommandAsync Command { get; private set; }

        /// <summary>
        /// 戦闘コマンドを設定します。
        /// </summary>
        /// <param name="command">設定する戦闘コマンド。</param>
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

            if (Command.Metadata.ProgressState.Equals(CommandProgressState.BeforeExecuted) ||
                Command.Metadata.ProgressState.Equals(CommandProgressState.Executed))
            {
                throw new InvalidOperationException("実行中のコマンドを上書きすることはできません。");
            }

            Command = command;
        }

        /// <summary>
        /// 戦闘コマンドを実行します。
        /// </summary>
        /// <param name="token">キャンセルトークン。</param>
        /// <returns>非同期操作。</returns>
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

                try
                {
                    await command.BeforeExecute(token);
                }
                catch (TaskCanceledException ex)
                {
                    UnityEngine.Debug.Log("UniTask BeforeExecute がキャンセルされました。");
                    _beforeExecuteSubject.OnErrorResume(ex);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"BeforeExecuteでエラーが発生しました: {ex}");
                    throw ex;
                }
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

                try
                {
                    await command.Execute(token);
                }
                catch (TaskCanceledException ex)
                {
                    UnityEngine.Debug.Log("UniTask Execute がキャンセルされました。");
                    _executeSubject.OnErrorResume(ex);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"Executeでエラーが発生しました: {ex}");
                    throw ex;
                }
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
                try
                {
                    await command.Complete(token);
                }
                catch (TaskCanceledException ex)
                {
                    UnityEngine.Debug.Log("UniTask Complete がキャンセルされました。");
                    _completeSubject.OnErrorResume(ex);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"Completeでエラーが発生しました: {ex}");
                    throw ex;
                }
            }
            else
            {
                UnityEngine.Debug.Log($"実行済みのCompleteをスキップしました。");
            }
        }
    }
}
