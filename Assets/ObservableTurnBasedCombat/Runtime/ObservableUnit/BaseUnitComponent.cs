using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    [Serializable]
    public abstract class BaseUnitComponent
    {
        public UnitComponentId Id { get => id; }
        [SerializeField] private UnitComponentId id = default;

        //public UnitComponentType Type { get; }


        public IEnumerable<UnitComponentId> RequiredLinks { get => _requiredLinks; }
        protected HashSet<UnitComponentId> _requiredLinks = new();


        // コンストラクタ
        protected BaseUnitComponent
            (
            UnitComponentId id,
            //UnitComponentType type,
            IEnumerable<UnitComponentId> links = null
            )
        {
            this.id = id;
            //Type = type;

            if (links != null) _requiredLinks = links.ToHashSet();
        }


        // エッジを追加するメソッド
        public bool AddRequiredLink(UnitComponentId id)
        {
            return _requiredLinks.Add(id);
        }
        public bool RemoveRequiredLink(UnitComponentId id)
        {
            return _requiredLinks.Remove(id);
        }
        public bool ContainsRequiredLink(UnitComponentId id)
        {
            return _requiredLinks.Contains(id);
        }
    }
}
