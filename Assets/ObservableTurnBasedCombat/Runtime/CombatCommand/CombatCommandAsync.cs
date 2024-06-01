using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ObservableTurnBasedCombat.BusinessLogic
{
    /// <summary>
    /// 非同期で実行可能な戦闘コマンドを表すクラスです。
    /// </summary>
    public class CombatCommandAsync
    {
        /// <summary>
        /// コマンドの一意の識別子を取得します。
        /// </summary>
        public CommandId Id { get; }

        /// <summary>
        /// コマンドに関連付けられた効果の識別子のリストを取得します。
        /// </summary>
        public List<CommandEffectId> EffectIds { get; }

        readonly Dictionary<CommandEffectId, ICombatCommandEffectAsync> _commandEffects;

        protected enum CommandState
        {
            NotStarted, // まだ実行されていない状態
            BeforeExecuteCalled, // BeforeExecute が呼び出された状態
            ExecuteCalled, // Execute が呼び出された状態
            Completed // Complete が呼び出された状態
        }
        private CommandState _state = CommandState.NotStarted;


        /// <summary>
        /// <see cref="CombatCommandAsync"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="id">コマンドの一意の識別子。</param>
        /// <param name="commandEffects">コマンドに関連付けられた効果のリスト。</param>
        public CombatCommandAsync(CommandId id, List<ICombatCommandEffectAsync> commandEffects)
        {
            Id = id;
            EffectIds = commandEffects.Select(ce => ce.Id).ToList();
            _commandEffects = commandEffects.ToDictionary(ce => ce.Id, ce => ce);
        }

        /// <summary>
        /// コマンドの実行前に実行する非同期処理を開始します。
        /// </summary>
        /// <param name="token">処理をキャンセルするためのトークン。</param>
        /// <returns>非同期操作を表すタスク。</returns>
        /// <exception cref="InvalidOperationException">BeforeExecute を 2 回以上呼び出すことはできません。</exception>
        public async UniTask BeforeExecute(CancellationToken token)
        {
            if (_state != CommandState.NotStarted)
                throw new InvalidOperationException("BeforeExecuteは既に呼び出されています");

            await ProcessEffects(token, action => action.BeforeExecute(token));

            _state = CommandState.BeforeExecuteCalled;
        }

        /// <summary>
        /// コマンドの実行処理を開始します。
        /// </summary>
        /// <param name="token">処理をキャンセルするためのトークン。</param>
        /// <returns>非同期操作を表すタスク。</returns>
        /// <exception cref="InvalidOperationException">Execute が呼び出されていないか、または順番に呼び出されていない場合にスローされます。</exception>
        public async UniTask Execute(CancellationToken token)
        {
            if (_state != CommandState.BeforeExecuteCalled)
                throw new InvalidOperationException("BeforeExecuteが呼び出されていないか、すでに実行済みです");

            await ProcessEffects(token, action => action.Execute(token));

            _state = CommandState.ExecuteCalled;
        }

        /// <summary>
        /// コマンドの実行完了後に実行する非同期処理を開始します。
        /// </summary>
        /// <param name="token">処理をキャンセルするためのトークン。</param>
        /// <returns>非同期操作を表すタスク。</returns>
        /// <exception cref="InvalidOperationException">Complete が呼び出されていないか、または順番に呼び出されていない場合にスローされます。</exception>
        public async UniTask Complete(CancellationToken token)
        {
            if (_state != CommandState.ExecuteCalled)
                throw new InvalidOperationException("Executeが呼び出されていないか、すでに実行済みです");

            await ProcessEffects(token, action => action.Complete(token));

            _state = CommandState.Completed;
        }

        private async UniTask ProcessEffects(CancellationToken token, Func<ICombatCommandEffectAsync, UniTask> action)
        {
            var taskList = new List<UniTask>();
            foreach (var commandEffect in _commandEffects.Values)
            {
                taskList.Add(action(commandEffect));
            }
            await UniTask.WhenAll(taskList);
        }
    }
}