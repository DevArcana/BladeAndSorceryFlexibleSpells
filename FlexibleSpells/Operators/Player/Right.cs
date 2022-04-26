using ThunderRoad;

namespace FlexibleSpells.Operators.Player
{
    public class Right : Operator
    {
        public RagdollHand Apply() => ThunderRoad.Player.currentCreature.handRight;
    }
}