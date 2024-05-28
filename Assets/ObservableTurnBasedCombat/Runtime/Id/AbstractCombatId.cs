using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace ObservableTurnBasedCombat
{
    /// <summary>
    /// 抽象クラス <c>AbstractCombatId</c> は、IDを表すための抽象基底クラスです。
    /// このクラスは <c>Serializable</c> 属性を持ち、<c>IEquatable<AbstractCombatId></c> インターフェースを実装しています。
    /// </summary>
    /// <remarks>
    /// <para>
    /// このクラスを継承して具体的な戦闘IDを表すクラスを作成する際には、IDと名前の初期化を行うためにコンストラクタを実装する必要があります。
    /// </para>
    /// <para>
    /// シリアライズ可能な形式でオブジェクトを保存したい場合は、<see cref="Serialize"/> メソッドを使用してバイト配列にシリアライズします。
    /// </para>
    /// <para>
    /// このクラスのオブジェクトは、<see cref="Equals(object)"/> メソッドおよび <see cref="GetHashCode"/> メソッドによって等値比較されます。
    /// </para>
    /// </remarks>
    [Serializable]
    public abstract class AbstractCombatId : IEquatable<AbstractCombatId>
    {
        [SerializeField] private int id;
        [SerializeField] private string name;

        /// <summary>
        /// <c>AbstractCombatId</c> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">IDの名前</param>
        protected AbstractCombatId(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        /// <summary>
        /// オブジェクトをJSON形式の文字列にシリアライズします。
        /// </summary>
        /// <returns>シリアライズされたJSON形式の文字列</returns>
        public string Serialize()
        {
            try
            {
                return JsonUtility.ToJson(this);
            }
            catch (Exception ex)
            {
                // シリアライズが失敗した場合に例外を投げる
                throw new SerializationException("Serialization failed.", ex);
            }
        }

        /// <summary>
        /// 指定されたJSON形式の文字列からオブジェクトをデシリアライズします。
        /// AbstractCombatId型にはデシリアライズできません。
        /// </summary>
        /// <typeparam name="T">デシリアライズされるオブジェクトの型</typeparam>
        /// <param name="json">デシリアライズするJSON形式の文字列</param>
        /// <returns>デシリアライズされたオブジェクト</returns>
        public static T Deserialize<T>(string json) where T : AbstractCombatId
        {
            if(typeof(AbstractCombatId) == typeof(T))
            {
                throw new ArgumentException($"{typeof(AbstractCombatId)}は引数に使用できません");
            }

            try
            {
                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception ex)
            {
                // デシリアライズが失敗した場合に例外を投げる
                throw new SerializationException("Deserialization failed.", ex);
            }
        }

        /// <summary>
        /// 指定されたオブジェクトが現在の <c>AbstractCombatId</c> インスタンスと等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">比較対象のオブジェクト</param>
        /// <returns>等しい場合は <c>true</c>、それ以外の場合は <c>false</c></returns>
        public bool Equals(AbstractCombatId other)
        {
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }

            return id == other.id && name == other.name;
        }

        /// <summary>
        /// 指定されたオブジェクトが現在の <c>AbstractCombatId</c> インスタンスと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns>等しい場合は <c>true</c>、それ以外の場合は <c>false</c></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as AbstractCombatId);
        }

        /// <summary>
        /// このインスタンスのハッシュコードを返します。
        /// </summary>
        /// <returns>このインスタンスのハッシュコード</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(id, name);
        }
    }
}
