using System;


namespace ObservableTurnBasedCombat.BusinessLogic
{
    /// <summary>
    /// ユニットの属性Idを表すクラスです。
    /// このクラスは、属性Idの識別子と名前を保持します。
    /// </summary>
    internal class AttributeId : IEquatable<AttributeId>
    {
        // 属性の識別子を表すプロパティ
        public int Id { get; }
        // 属性の名前を表すプロパティ
        public string Name { get; }


        // コンストラクタ
        public AttributeId(int id, string name)
        {
            Id = id;
            Name = name;
        }


        // Equalsメソッドの実装
        public override bool Equals(object obj)
        {
            return Equals(obj as AttributeId);
        }
        // IEquatable<T> インターフェースのメソッドの実装
        public bool Equals(AttributeId other)
        {
            if (other == null) return false;

            return Name == other.Name && Id == other.Id;
        }
        // GetHashCodeメソッドの実装
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
