
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.ObservableUnit
{
    using Application;

    public class CombatUnitConfigStorageRepositoryService : IObservableUnitRepository
    {
        private CombatUnitConfigStorage _storage;


        public CombatUnitConfigStorageRepositoryService(CombatUnitConfigStorage storage)
        {
            _storage = storage;
        }


        public ObservableUnit FindById(UnitId id) 
        { 
            return new ObservableUnit(new UnitId(1, "Test")); 
        }


        public IEnumerable<ObservableUnit> FindAll() 
        {
            var list = new List<ObservableUnit>();
            foreach (var item in list) yield return item;
        }


        public void Save(ObservableUnit unit) { }
        public void Delete(ObservableUnit unit) { }
    }
}
