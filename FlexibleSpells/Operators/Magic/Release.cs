using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace FlexibleSpells.Operators.Magic
{
    public class Release : Operator
    {
        public override bool Terminal => true;

        public void Apply(Handle a, Handle b)
        {
            var joints = new List<FixedJoint>();
            joints.AddRange(a.rb.gameObject.GetComponents<FixedJoint>());
            joints.AddRange(b.rb.gameObject.GetComponents<FixedJoint>());

            foreach (var joint in joints.Where(x => x.connectedBody == a.rb || x.connectedBody == b.rb))
            {       
                Object.Destroy(joint);
            }
        }
        
        public void Apply(Handle handle)
        {
            foreach (var joint in handle.rb.gameObject.GetComponents<FixedJoint>())
            {       
                Object.Destroy(joint);
            }
        }
        
        public void Apply(Side ha, Side hb)
        {
            var a = ThunderRoad.Player.currentCreature.GetHand(ha).grabbedHandle;
            var b = ThunderRoad.Player.currentCreature.GetHand(hb).grabbedHandle;
            
            if (a == null || b == null)
            {
                return;
            }
            
            var joints = new List<FixedJoint>();
            joints.AddRange(a.rb.gameObject.GetComponents<FixedJoint>());
            joints.AddRange(b.rb.gameObject.GetComponents<FixedJoint>());

            foreach (var joint in joints.Where(x => x.connectedBody == a.rb || x.connectedBody == b.rb))
            {       
                Object.Destroy(joint);
            }
        }
        
        public void Apply(Side side)
        {
            var handle = ThunderRoad.Player.currentCreature.GetHand(side).grabbedHandle;
            
            if (handle == null)
            {
                return;
            }
            
            foreach (var joint in handle.rb.gameObject.GetComponents<FixedJoint>())
            {       
                Object.Destroy(joint);
            }
        }
    }
}