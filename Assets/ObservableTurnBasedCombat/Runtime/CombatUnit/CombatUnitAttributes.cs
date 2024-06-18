using ObservableTurnBasedCombat.Application;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObservableTurnBasedCombat
{
    public class CombatUnitAttributes : ICombatUnitAttribute
    {
        public UnitAttributeId Id { get; protected set; }


        private Dictionary<UnitAttributeId, ICombatUnitAttribute> _attributeById = new Dictionary<UnitAttributeId, ICombatUnitAttribute>();


        public CombatUnitAttributes(UnitAttributeId id)
        {
            Id = id;
        }
        public CombatUnitAttributes
        (
            UnitAttributeId id,
            IEnumerable<ICombatUnitAttribute> attributes
        ) 
            : this(id)
        {
            foreach ( var attribute in attributes )
            {
                _attributeById.Add(attribute.Id, attribute);
            }
        }

        public bool TryGetById(UnitAttributeId id, out ICombatUnitAttribute attribute)
        {
            if (Id.Equals(id))
            {
                attribute = this;
                return true;
            }


            if (ContainsById(id))
            {
                attribute = _attributeById[id];
                return true;
            }


            attribute = null;
            return false;
        }
        public bool ContainsById(UnitAttributeId id)
        {
            return _attributeById.Keys.Contains(id);
        }
        public bool RemoveById(UnitAttributeId id)
        {
            return _attributeById.Remove(id);
        }

        public bool Contains(ICombatUnitAttribute attribute)
        {
            return ContainsById(attribute.Id);
        }
        public void Add(ICombatUnitAttribute attribute)
        {
            try
            {
                _attributeById.Add(attribute.Id, attribute);
            }
            catch (ArgumentException ex)
            {
                Debug.LogError("同じIdを持つアトリビュートを追加することはできません。");
                throw ex;
            }
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
