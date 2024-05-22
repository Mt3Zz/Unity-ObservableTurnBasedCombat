using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


/* Test List
- [] ユニットは属性（Attribute）を持つ
    - [] ユニット共通の属性を持つ
    - [] ユニット別の属性を持つ

- [] ユニットはステータス（Stats）を持つ
    - [] ユニット共通のステータスを持つ
    - [] ユニット別のステータスを持つ

- [] ユニットはバフ・デバフを持つ
    - [] 
    - [] 


- [] ダメージ計算式に応じてHPを減らす
- [] HPが0であることを判定する

- [] バフの量に応じてステータスを増やす
- [] デバフの量に応じてステータスを減らす

*/


namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    public class CombatUnitBaseTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void CombatUnitBaseTestSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator CombatUnitBaseTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
