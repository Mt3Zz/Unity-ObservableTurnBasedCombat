using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnitAttribute<T> : ICombatUnitAttribute
        where T : struct
    {
        public UnitAttributeId Id { get; protected set; }
        public T Value { get; protected set; }



        public CombatUnitAttribute(UnitAttributeId id)
        {
            Id = id;
        }


        public bool TryGetById(UnitAttributeId id, out ICombatUnitAttribute attribute)
        {
            if (Id.Equals(id))
            {
                attribute = this;
                return true;
            }

            attribute = null;
            return false;
        }
        public bool Contains(UnitAttributeId id)
        {
            return Id.Equals(id);
        }

        public void Add(ICombatUnitAttribute attribute)
        {
            // 単体のアトリビュートでは追加をサポートしない
            throw new InvalidOperationException("単体のアトリビュートにアトリビュートを追加することはできません。");
        }
        public void Remove(ICombatUnitAttribute attribute)
        {
            // 単体のアトリビュートでは削除をサポートしない
            throw new InvalidOperationException("単体のアトリビュートからアトリビュートを削除することはできません。");
        }


        /// <summary>
        /// 指定されたオブジェクトが現在の <c>AbstractCombatUnitAttribute</c> インスタンスと等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">比較対象のオブジェクト</param>
        /// <returns>等しい場合は <c>true</c>、それ以外の場合は <c>false</c></returns>
        public bool Equals(ICombatUnitAttribute other)
        {
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        /// <summary>
        /// 指定されたオブジェクトが現在の <c>AbstractCombatUnitAttribute</c> インスタンスと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns>等しい場合は <c>true</c>、それ以外の場合は <c>false</c></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ICombatUnitAttribute);
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
