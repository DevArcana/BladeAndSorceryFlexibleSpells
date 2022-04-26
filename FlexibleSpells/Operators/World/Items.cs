using ThunderRoad;
using UnityEngine;

namespace FlexibleSpells.Operators.World
{
    public class Items : Operator
    {
        public Item[] Apply() => Object.FindObjectsOfType<Item>();
    }
}