using System;
using System.Collections.Generic;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnitStatsTerm<T> : BaseCombatUnitComponent
        where T : struct, IComparable, IFormattable, IConvertible, IEquatable<T>, IComparable<T> // T‚ð’lŒ^‚ÉŒÀ’è
    {
        public T BaseValue { get; }
        public StatsTermType TermType { get; }


        List<CombatUnitStatsModifier> _modifiers = new();


        public CombatUnitStatsTerm(UnitStatsTermId id, T baseValue , StatsTermType type)
            :base(id, UnitComponentType.StatsTerm)
        {
            BaseValue = baseValue;
            TermType = type;
        }


        public T CalculateValue(T defaultValue)
        {
            var result = BaseValue;


            if (null == _modifiers || 0 == _modifiers.Count) return result;


            switch (TermType)
            {
                case StatsTermType.Addition:
                    break;
                case StatsTermType.Multiplication:
                    break;
            }


            return result;
        }
    }
    public enum StatsTermType
    {
        Addition,
        Multiplication
    }
}
