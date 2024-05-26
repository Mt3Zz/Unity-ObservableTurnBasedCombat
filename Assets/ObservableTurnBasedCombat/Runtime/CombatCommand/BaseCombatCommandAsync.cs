using System;
using R3;

namespace ObservableTurnBasedCombat.BusinessLogic
{
    /// <summary>
    /// 戦闘コマンドの抽象クラスです。
    /// 戦闘における行動の基本的な構造を提供します。
    /// </summary>
    public abstract class BaseCombatCommandAsync : IDisposable
    {
        // BeforeExecute、Execute、Completeの各イベントを発行するためのSubject
        private readonly Subject<Unit> _beforeExecuteSubject = new Subject<Unit>();
        private readonly Subject<Unit> _executeSubject = new Subject<Unit>();
        private readonly Subject<Unit> _completeSubject = new Subject<Unit>();

        /// <summary>
        /// 戦闘コマンドの各メソッドのObservableを返します。
        /// </summary>
        public (
            Observable<Unit> BeforeExecute,
            Observable<Unit> Execute,
            Observable<Unit> Complete
        ) ObservableMethods =>
        (
            _beforeExecuteSubject,
            _executeSubject,
            _completeSubject
        );

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
        /// コマンドの実行前に呼び出されます。
        /// </summary>
        public void BeforeExecute()
        {
            _beforeExecuteSubject.OnNext(Unit.Default);
            OnBeforeExecute();
        }

        /// <summary>
        /// コマンドを実行します。
        /// </summary>
        public void Execute()
        {
            _executeSubject.OnNext(Unit.Default);
            OnExecute();
        }

        /// <summary>
        /// コマンドの完了時に呼び出されます。
        /// </summary>
        public void Complete()
        {
            _completeSubject.OnNext(Unit.Default);
            OnComplete();
        }

        /// <summary>
        /// コマンドの実行前に派生クラスで実装されるメソッドです。
        /// </summary>
        protected abstract void OnBeforeExecute();

        /// <summary>
        /// コマンドを実行する際に派生クラスで実装されるメソッドです。
        /// </summary>
        protected abstract void OnExecute();

        /// <summary>
        /// コマンドが完了したときに派生クラスで実装されるメソッドです。
        /// </summary>
        protected abstract void OnComplete();
    }
}
