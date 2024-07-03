

namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    public class FakeComponentNode : IComponentNode
    {
        public bool Equals(IComponentNode other)
        {
            if (other == null || GetType() != other.GetType()) return false;
            return GetHashCode() == other.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as FakeComponentNode);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
