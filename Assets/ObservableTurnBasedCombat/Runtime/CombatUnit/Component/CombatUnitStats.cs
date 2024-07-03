using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnitStats<T> : CombatUnitComponent
        where T : struct
    {
        public T BaseValue { get; }

        public static T InitialValue { get; }
        public static T Default { get; }


        //List<ICombatUnitStatsTerm> Terms { get; }
        //List<ICombatUnitStatsModifier> Modefiers { get; }


        public CombatUnitStats(UnitStatId id)
            : base(id, UnitComponentType.Stats)
        {
            
        }

        public virtual T ApplyModifier()
        {
            return BaseValue;
        }
    }
}
