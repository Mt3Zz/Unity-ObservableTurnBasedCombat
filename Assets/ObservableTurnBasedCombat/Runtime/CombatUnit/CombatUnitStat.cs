using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnitStat<T> : ICombatUnitStat<T>
        where T : struct
    {
        public UnitStatId Id { get; protected set; }

        public T Value { get; protected set; }

        public static T InitialValue {  get; protected set; } = default;
        public static T Default { get; protected set; } = default;


        public CombatUnitStat(UnitStatId id)
        {
            Id = id;
        }


        public bool TryGetById(UnitStatId id, out ICombatUnitStat stat)
        {
            if (Id.Equals(id))
            {
                stat = this;
                return true;
            }

            stat = null;
            return false;
        }


        public void Add(ICombatUnitStat stat)
        {
            // 単体のステータスでは追加をサポートしない
            throw new InvalidOperationException("単体のステータスにステータスを追加することはできません。");
        }
        public bool RemoveById(UnitStatId id)
        {
            // 単体のステータスでは削除をサポートしない
            throw new InvalidOperationException("単体のステータスからステータスを削除することはできません。");
        }


        public bool Contains(UnitStatId id)
        {
            return Id.Equals(id);
        }
        public bool Contains(ICombatUnitStat stat)
        {
            return Id.Equals(stat.Id);
        }


        /// <summary>
        /// 指定されたオブジェクトが現在の <c>AbstractCombatUnitStat</c> インスタンスと等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">比較対象のオブジェクト</param>
        /// <returns>等しい場合は <c>true</c>、それ以外の場合は <c>false</c></returns>
        public bool Equals(ICombatUnitStat other)
        {
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        /// <summary>
        /// 指定されたオブジェクトが現在の <c>AbstractCombatUnitStat</c> インスタンスと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns>等しい場合は <c>true</c>、それ以外の場合は <c>false</c></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ICombatUnitStat);
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
