using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnitStats<T> : ObservableUnitComponent
        where T : struct, IComparable, IFormattable, IConvertible, IEquatable<T>, IComparable<T> // T‚ð’lŒ^‚ÉŒÀ’è
    {
        public T BaseValue { get; }

        public static T InitialValue { get; }
        public static T DefaultValue { get; }


        List<CombatUnitStatsTerm<T>> Terms { get; }
        //List<ICombatUnitStatsModifier> Modefiers { get; }


        //private ComponentGraph<CombatUnitStatsTerm> graph = new();


        public CombatUnitStats(UnitComponentId id)
            : base(id)
        { }

        public virtual T ApplyModifier()
        {
            return BaseValue;
        }
    }
}
