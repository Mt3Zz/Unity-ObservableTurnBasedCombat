using System;
using System.Collections.Generic;
using System.Linq;

namespace ObservableTurnBasedCombat.Application
{
    public abstract class BaseCombatUnitComponent : IComponentNode
    {
        public UnitComponentId Id { get; }
        public UnitComponentType Type { get; }

        //protected HashSet<UnitComponentId> _requiredLinks = new();
        //public IEnumerable<UnitComponentId> RequiredLinks { get => _requiredLinks; }

        protected HashSet<UnitComponentId> _linkedIds = new();
        public IEnumerable<UnitComponentId> LinkedIds { get => _linkedIds; }


        // コンストラクタ
        protected BaseCombatUnitComponent
            (
            UnitComponentId id, 
            UnitComponentType type,
            IEnumerable<UnitComponentId> links = null
            )
        {
            Id = id;
            Type = type;

            if (links != null)  _linkedIds = links.ToHashSet();
            else                _linkedIds = new HashSet<UnitComponentId>();
        }


        // エッジを追加するメソッド
        public bool AddLink(UnitComponentId id)
        {
            return _linkedIds.Add(id);
        }
        public bool RemoveLink(UnitComponentId id)
        {
            return _linkedIds.Remove(id);
        }
        public bool ContainsLink(UnitComponentId id)
        {
            return _linkedIds.Contains(id);
        }


        /// <summary>
        /// 指定されたオブジェクトが現在の <c>IComponentNode</c> インスタンスと等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">比較対象のオブジェクト</param>
        /// <returns>等しい場合は <c>true</c>、それ以外の場合は <c>false</c></returns>
        public bool Equals(IComponentNode other)
        {
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }

            return GetHashCode() == other.GetHashCode();
        }
        /// <summary>
        /// 指定されたオブジェクトが現在の <c>IComponentNode</c> インスタンスと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns>等しい場合は <c>true</c>、それ以外の場合は <c>false</c></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as IComponentNode);
        }

        /// <summary>
        /// このインスタンスのハッシュコードを返します。
        /// </summary>
        /// <returns>このインスタンスのハッシュコード</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Type);
        }
    }
    public struct UnitComponentType : IEquatable<UnitComponentType>
    {
        public static readonly UnitComponentType Attribute = new(101);

        public static readonly UnitComponentType Stats = new(201);
        public static readonly UnitComponentType StatsTerm = new(202);
        public static readonly UnitComponentType StatsModefier = new(203);

        public static readonly UnitComponentType Timer = new(301);

        public int State { get; }
        private UnitComponentType(int state)
        {
            State = state;
        }

        public bool Equals(UnitComponentType other)
        {
            return State == other.State;
        }
    }
}
