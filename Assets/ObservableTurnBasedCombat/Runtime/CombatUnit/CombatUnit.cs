using ObservableCollections;
using System;
using System.Collections.Generic;


namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnit
    {
        public UnitId Id { get => Metadata.Id; }
        public UnitMetadata Metadata { get; protected set; }


        private ICombatUnitRepository _repository;


        public CombatUnit(UnitId id, ICombatUnitRepository repository)
        {
            Metadata.SetId(id);


            _repository = repository;
        }


        public ComponentGraph<UnitComponentId> Graph { get; protected set; }
        public ObservableDictionary<UnitComponentId, BaseCombatUnitComponent> ComponentById { get; protected set; }



        //*
        public T GetStatsValueById<T>(UnitStatsId id)
            where T : struct, IComparable, IFormattable, IConvertible, IEquatable<T>, IComparable<T> // T‚ð’lŒ^‚ÉŒÀ’è
        {
            var stats = ComponentById[id];
            var neighborNodeIds = Graph.GetNeighborNodes(id);


            T result = default;
            foreach (var neighborNodeId in neighborNodeIds)
            {
                var neighborNode = ComponentById[neighborNodeId];
                if (!neighborNode.Type.Equals(UnitComponentType.StatsModefier)) continue;



            }


            return result;
        }
        //*/
    }
}
