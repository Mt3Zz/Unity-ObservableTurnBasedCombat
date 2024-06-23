using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnitStats : ICombatUnitStat
    {
        public UnitStatId Id { get; protected set; }


        private Dictionary<UnitStatId, ICombatUnitStat> _statById = new Dictionary<UnitStatId, ICombatUnitStat> ();


        public CombatUnitStats(UnitStatId id)
        {
            Id = id;
        }
        public CombatUnitStats
        (
            UnitStatId id,
            IEnumerable<ICombatUnitStat> stats
        )
            : this(id)
        {
            foreach (var stat in stats)
            {
                _statById.Add(stat.Id, stat);
            }
        }


        public bool TryGetById(UnitStatId id, out ICombatUnitStat stat)
        {
            if (Id.Equals(id))
            {
                stat = this;
                return true;
            }


            if (Contains(id))
            {
                stat = _statById[id];
                return true;
            }


            stat = null;
            return false;
        }

        public void Add(ICombatUnitStat stat)
        {
            try
            {
                _statById.Add(stat.Id, stat);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("同じIdを持つステータスを追加することはできません。", ex);
            }
        }
        public bool RemoveById(UnitStatId id)
        {
            return _statById.Remove(id);
        }

        public bool Contains(UnitStatId id)
        {
            return _statById.Keys.Contains(id);
        }
        public bool Contains(ICombatUnitStat stat)
        {
            return Contains(stat.Id);
        }



        /// <summary>
        /// 指定されたオブジェクトが現在の <c>CombatUnitStats</c> インスタンスと等しいかどうかを判断します。
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
        /// 指定されたオブジェクトが現在の <c>CombatUnitStats</c> インスタンスと等しいかどうかを判断します。
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
