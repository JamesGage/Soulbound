using System;
using RPG.Stats;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Bond Effect", menuName = "Abilities/Effects/Bond")]
    public class BondEffect : EffectStrategy
    {
        [Range(0, 100)]
        [SerializeField] private int bondChange;
        [SerializeField] private string filterTag;

        public override void StartEffect(AbilityData data, Action finished)
        {
            foreach (var target in data.GetTargets())
            {
                if (target.CompareTag(filterTag))
                {
                    var bond = data.GetUser().GetComponent<Bond>();
                    if (bond)
                    {
                        bond.AddBond(bondChange);
                        data.StartCoroutine(bond.PauseBondDegrade());
                    }
                }
            }

            finished();
        }
    }
}