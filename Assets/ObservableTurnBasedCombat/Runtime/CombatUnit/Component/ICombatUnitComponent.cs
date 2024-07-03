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


        HashSet<ICombatUnitComponent> Links { get; }


        bool AddLink(ICombatUnitComponent link);
        bool RemoveLink(ICombatUnitComponent link);
        bool ContainsLink(ICombatUnitComponent link);
    }

    public struct UnitComponentType : IEquatable<UnitComponentType>
    {
        public static readonly UnitComponentType Attribute = new(0);
        public static readonly UnitComponentType Stats = new(1);
        public static readonly UnitComponentType StatsModefier = new(2);
        public static readonly UnitComponentType Timer = new(3);

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
