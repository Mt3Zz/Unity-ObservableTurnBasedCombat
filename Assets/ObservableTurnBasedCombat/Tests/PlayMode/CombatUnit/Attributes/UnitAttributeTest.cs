using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


/*
Test List
- [] Id‚ª“¯‚¶Attribute‚ð•Ô‚·
- [] Id‚ªˆá‚¤Attribute‚È‚ç—áŠO‚ð“Š‚°‚é
 
*/

namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    using BusinessLogic;

    public class UnitAttributeTest
    {
        [Test]
        public void Equals_SameObject_ReturnTrue()
        {
            // Arrange
            int id = 0;
            string name = "Name";
            var attributeId = new AttributeId(id, name);

            var detail = new FakeDetail();
            
            var attribute = new UnitAttribute(attributeId, detail);


            // Act
            // Nothing to do


            // Assert
            //Assert.That(attribute.Equals
        }
    }
}
