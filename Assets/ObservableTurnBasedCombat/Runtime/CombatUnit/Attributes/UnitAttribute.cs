using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.BusinessLogic
{
    internal class UnitAttribute
    {
        public AttributeId Id { get; }
        public AttributeDetailBase Detail { get; }


        public UnitAttribute(AttributeId id, AttributeDetailBase detail)
        {
            Id = id;
            Detail = detail;
        }




        public UnitAttribute FindById(AttributeId id)
        {
            if (!Id.Equals(id))
            {
                throw new InvalidOperationException($"Attribute with id {id.Name} not found");
            }
            return this;
        }
    }
}
