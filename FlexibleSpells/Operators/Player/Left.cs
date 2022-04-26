using ThunderRoad;

namespace FlexibleSpells.Operators.Player
{
    public class Left : Operator
    {
        public RagdollHand Apply() => ThunderRoad.Player.currentCreature.handLeft;
    }
}