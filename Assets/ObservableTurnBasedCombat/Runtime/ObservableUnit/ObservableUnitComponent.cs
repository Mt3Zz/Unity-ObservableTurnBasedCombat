using System;
using System.Collections.Generic;
using UnityEngine;

using R3;


namespace ObservableTurnBasedCombat.ObservableUnit
{
    [Serializable]
    public class ObservableUnitComponent : BaseUnitComponent, IEquatable<ObservableUnitComponent>, IDisposable
    {
        // UniRxを使用して変更通知するSubject
        public Observable<ObservableUnitComponent> OnComponentChanged => _componentChangedSubject;
        private Subject<ObservableUnitComponent> _componentChangedSubject = new();


        // コンストラクタ
        public ObservableUnitComponent(
            UnitComponentId id,
            IEnumerable<UnitComponentId> links = null
        )
            : base(id, links)
        { }


        public void Dispose()
        {
            _componentChangedSubject.Dispose();
        }


        // コンポーネントの変更を通知するメソッド
        protected void NotifyComponentChanged()
        {
            // Debug用Log表示
            //UnityEngine.Debug.Log("Calls UnitComponent.NotifyComponentChanged");

            _componentChangedSubject.OnNext(this);
        }


        /// <summary>
        /// 指定されたオブジェクトが現在の <c>UnitComponent</c> インスタンスと等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">比較対象のオブジェクト</param>
        /// <returns>等しい場合は <c>true</c>、それ以外の場合は <c>false</c></returns>
        public bool Equals(ObservableUnitComponent other)
        {
            if (other == null || GetType() != other.GetType()) return false;
            return GetHashCode() == other.GetHashCode();
        }
        /// <summary>
        /// 指定されたオブジェクトが現在の <c>UnitComponent</c> インスタンスと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns>等しい場合は <c>true</c>、それ以外の場合は <c>false</c></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ObservableUnitComponent);
        }
        /// <summary>
        /// このインスタンスのハッシュコードを返します。
        /// </summary>
        /// <returns>このインスタンスのハッシュコード</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
