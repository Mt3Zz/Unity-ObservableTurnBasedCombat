using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    public interface ICombatUnitComponent : IEquatable<ICombatUnitComponent>
    {
        UnitComponentId Id { get; }


        UnitComponentType Type { get; }
        List<CombatUnitComponentLink> Links { get; }


        void AddLink(CombatUnitComponentLink link);
    }

    public enum UnitComponentType
    {
        Attribute,
        Stat,
        StatModefier,
        Timer
    }
}
