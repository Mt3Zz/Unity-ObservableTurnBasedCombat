using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    public interface ICombatUnitRepository
    {
        CombatUnit FindById(UnitId id);
        IEnumerable<CombatUnit> FindAll();


        void Save(CombatUnit unit);
        void Delete(CombatUnit unit);
    }
}
