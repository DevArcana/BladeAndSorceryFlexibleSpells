using ThunderRoad;

namespace FlexibleSpells.Operators.Player
{
    public class Item : Operator
    {
        public Handle Apply(RagdollHand hand) => hand.grabbedHandle;
    }
}