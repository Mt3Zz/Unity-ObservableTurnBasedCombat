using ObservableTurnBasedCombat.Application;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    public class FakeUnitRepository : IUnitRepository
    {
        public UnitComponentId Id { get; }
        public ObservableUnitComponent Component { get; }


        public FakeUnitRepository(ObservableUnitComponent component)
        {
            Id = component.Id;
            Component = component;
        }


        public ObservableUnitComponent FetchComponentById(UnitComponentId id)
        {
            if(Id.Equals(id)) return Component;
            return null;
        }
    }
}
