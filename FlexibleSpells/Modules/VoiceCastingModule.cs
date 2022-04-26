using System.Collections;
using FlexibleSpells.VoiceCasting;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FlexibleSpells.Modules
{
    public class OperatorRegistration
    {
        public string word;
        public string path;
    }
    
    public class VoiceCastingModule : LevelModule
    {
        public OperatorRegistration[] operators;

        public override IEnumerator OnLoadCoroutine()
        {
            EventManager.onPossess += OnPlayerPossess;
            return base.OnLoadCoroutine();
        }

        private void OnPlayerPossess(Creature creature, EventTime eventtime)
        {
            Debug.Log("Initializing voice casting");
            
            foreach (var obj in Object.FindObjectsOfType<VoiceCastingBehaviour>())
            {
                Debug.Log("Removing already existing voice casting behaviour");
                Object.Destroy(obj);
            }

            var display = Addressables
                .InstantiateAsync("DevArcana.FlexibleSpells.Prefabs.SpellDisplay", creature.centerEyes.transform)
                .WaitForCompletion()
                .GetComponentInChildren<TextMesh>();

            display.text = "";
            
            OperatorDictionary.Populate(operators);
            var spellbinding = creature.gameObject.AddComponent<VoiceCastingBehaviour>();
            spellbinding.display = display;
        }
    }
}