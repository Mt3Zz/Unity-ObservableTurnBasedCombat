using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnitAttribute<T> : ICombatUnitAttribute
        where T : struct
    {
        public UnitAttributeId Id { get; }



        public CombatUnitAttribute(UnitAttributeId id)
        {
            Id = id;
        }




    }
}
