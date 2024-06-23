using ObservableTurnBasedCombat.Application;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnitComponent : ICombatUnitComponent
    {
        public UnitComponentId Id { get; }
        public UnitComponentType Type { get; private set; }
        public List<CombatUnitComponentLink> Links { get; }

        // コンストラクタ
        public CombatUnitComponent(UnitComponentId id, UnitComponentType type)
        {
            Id = id;
            Type = type;
            Links = new List<CombatUnitComponentLink>();
        }

        // エッジを追加するメソッド
        public void AddLink(CombatUnitComponentLink link)
        {
            Links.Add(link);
        }


        /// <summary>
        /// 指定されたオブジェクトが現在の <c>ICombatUnitComponent</c> インスタンスと等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">比較対象のオブジェクト</param>
        /// <returns>等しい場合は <c>true</c>、それ以外の場合は <c>false</c></returns>
        public bool Equals(ICombatUnitComponent other)
        {
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        /// <summary>
        /// 指定されたオブジェクトが現在の <c>ICombatUnitComponent</c> インスタンスと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns>等しい場合は <c>true</c>、それ以外の場合は <c>false</c></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ICombatUnitComponent);
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
