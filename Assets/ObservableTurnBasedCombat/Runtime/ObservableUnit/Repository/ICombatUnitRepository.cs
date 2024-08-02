using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    public interface IObservableUnitRepository
    {
        ObservableUnit FindById(UnitId id);
        IEnumerable<ObservableUnit> FindAll();


        void Save(ObservableUnit unit);
        void Delete(ObservableUnit unit);
    }
}
