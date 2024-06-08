using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ObservableTurnBasedCombat.Application
{
    /// <summary>
    /// 非同期で実行可能な戦闘コマンドを表すクラスです。
    /// </summary>
    public class BaseCombatCommandAsync
    {
        /// <summary>
        /// コマンドの識別子を取得します。
        /// </summary>
        public CommandId Id { get => Metadata.Id; }
        /// <summary>
        /// コマンドのメタデータを取得します。
        /// </summary>
        public CommandMetadata Metadata { get => _editableMetadata; }
        internal EditableCommandMetadata _editableMetadata = new EditableCommandMetadata();

        protected List<ICombatCommandEffectAsync> _effects;


        protected enum ProgressState
        {
            NotStarted, // まだ実行されていない状態
            BeforeExecuteCalled, // BeforeExecute が呼び出された状態
            ExecuteCalled, // Execute が呼び出された状態
            Completed // Complete が呼び出された状態
        }
        protected ProgressState _state = ProgressState.NotStarted;


        /// <summary>
        /// <see cref="BaseCombatCommandAsync"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="id">コマンドの一意の識別子。</param>
        /// <param name="commandEffects">コマンドに関連付けられた効果のリスト。</param>
        public BaseCombatCommandAsync(CommandId id, List<ICombatCommandEffectAsync> commandEffects)
        {
            _editableMetadata.SetId(id);
            _editableMetadata.SetEffectIds(commandEffects.Select(ce => ce.Id).ToList());

            _effects = commandEffects;
        }
        public BaseCombatCommandAsync(BaseCombatCommandAsync command)
        {
            _editableMetadata = command._editableMetadata;

            _effects = command._effects;
        }

        /// <summary>
        /// コマンドの実行前に実行する非同期処理を開始します。
        /// </summary>
        /// <param name="token">処理をキャンセルするためのトークン。</param>
        /// <returns>非同期操作を表すタスク。</returns>
        /// <exception cref="InvalidOperationException">BeforeExecute を 2 回以上呼び出すことはできません。</exception>
        public async UniTask BeforeExecute(CancellationToken token)
        {
            if (_state != ProgressState.NotStarted)
                throw new InvalidOperationException("BeforeExecuteは既に呼び出されています");

            await ProcessEffects(token, action => action.BeforeExecute(token));

            _state = ProgressState.BeforeExecuteCalled;
        }

        /// <summary>
        /// コマンドの実行処理を開始します。
        /// </summary>
        /// <param name="token">処理をキャンセルするためのトークン。</param>
        /// <returns>非同期操作を表すタスク。</returns>
        /// <exception cref="InvalidOperationException">Execute が呼び出されていないか、または順番に呼び出されていない場合にスローされます。</exception>
        public async UniTask Execute(CancellationToken token)
        {
            if (_state != ProgressState.BeforeExecuteCalled)
                throw new InvalidOperationException("BeforeExecuteが呼び出されていないか、すでに実行済みです");

            await ProcessEffects(token, action => action.Execute(token));

            _state = ProgressState.ExecuteCalled;
        }

        /// <summary>
        /// コマンドの実行完了後に実行する非同期処理を開始します。
        /// </summary>
        /// <param name="token">処理をキャンセルするためのトークン。</param>
        /// <returns>非同期操作を表すタスク。</returns>
        /// <exception cref="InvalidOperationException">Complete が呼び出されていないか、または順番に呼び出されていない場合にスローされます。</exception>
        public async UniTask Complete(CancellationToken token)
        {
            if (_state != ProgressState.ExecuteCalled)
                throw new InvalidOperationException("Executeが呼び出されていないか、すでに実行済みです");

            await ProcessEffects(token, action => action.Complete(token));

            _state = ProgressState.Completed;
        }

        private async UniTask ProcessEffects(CancellationToken token, Func<ICombatCommandEffectAsync, UniTask> action)
        {
            var taskList = new List<UniTask>();
            foreach (var commandEffect in _effects)
            {
                taskList.Add(action(commandEffect));
            }
            await UniTask.WhenAll(taskList);
        }
    }
}