using ThunderRoad;

namespace FlexibleSpells.Operators.Magic
{
    public class Ignite : Operator
    {
        public override bool Terminal => true;
        
        private readonly SpellCastCharge _charge = Catalog.GetData<SpellCastCharge>("Fire");

        public void Apply(Handle handle)
        {
            var item = handle.item;
            if (item != null)
            {
                foreach (var colliderGroup in item.colliderGroups)
                {
                    var imbue = colliderGroup.imbue;

                    if (imbue != null)
                    {
                        imbue.Transfer(_charge, _charge.imbueRate * 10.0f);
                    }
                }
            }
        }
        
        public void Apply(Item[] items)
        {
            foreach (var item in items)
            {
                foreach (var colliderGroup in item.colliderGroups)
                {
                    var imbue = colliderGroup.imbue;

                    if (imbue != null)
                    {
                        imbue.Transfer(_charge, _charge.imbueRate * 10.0f);
                    }
                }
            }
        }
    }
}