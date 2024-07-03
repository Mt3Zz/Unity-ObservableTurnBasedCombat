
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.DataAccess
{
    using Application;

    public class CombatUnitConfigStorageRepositoryService : ICombatUnitRepository
    {
        private CombatUnitConfigStorage _storage;


        public CombatUnitConfigStorageRepositoryService(CombatUnitConfigStorage storage)
        {
            _storage = storage;
        }


        public CombatUnit FindById(UnitId id) { return new CombatUnit(new UnitId(1, "Test"), this); }
        public IEnumerable<CombatUnit> FindAll() 
        {
            var list = new List<CombatUnit>();
            foreach (var item in list) yield return item;
        }


        public void Save(CombatUnit unit) { }
        public void Delete(CombatUnit unit) { }
    }
}
