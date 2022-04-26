using ThunderRoad;
using UnityEngine;

namespace FlexibleSpells.Operators.Magic
{
    public class Bind : Operator
    {
        public override bool Terminal => true;
        
        public void Apply(Handle a, Handle b)
        {
            var joint = a.rb.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = b.rb;
            joint.anchor = a.transform.position;
            joint.connectedAnchor = b.transform.position;
        }
        
        public void Apply(Side ha, Side hb)
        {
            var a = ThunderRoad.Player.currentCreature.GetHand(ha).grabbedHandle;
            var b = ThunderRoad.Player.currentCreature.GetHand(hb).grabbedHandle;

            if (a == null || b == null)
            {
                return;
            }
            
            var joint = a.rb.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = b.rb;
            joint.anchor = a.transform.position;
            joint.connectedAnchor = b.transform.position;
        }
    }
}