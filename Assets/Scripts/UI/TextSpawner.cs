using RPG.Combat;
using UnityEngine;

namespace RPG.UI
{
    public class TextSpawner : MonoBehaviour
    {
        [SerializeField] GameObject _damageTextPrefab = null;
        
        private DamageText _damageText;

        public void SpawnDamage(int value, DamageType damageType, bool isCrititcal, WeaponConfig weapon)
        {
            var instance = Instantiate(_damageTextPrefab, transform);
            instance.GetComponentInChildren<DamageText>().SetDamage(value, damageType, isCrititcal, weapon);
        }
    }
}