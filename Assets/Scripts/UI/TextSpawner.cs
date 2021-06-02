using RPG.Combat;
using UnityEngine;

namespace RPG.UI
{
    public class TextSpawner : MonoBehaviour
    {
        [SerializeField] GameObject _damageTextPrefab = null;
        
        private DamageText _damageText;

        public void SpawnDamage(float value, DamageType damageType)
        {
            var instance = Instantiate(_damageTextPrefab, transform);
            instance.GetComponentInChildren<DamageText>().SetDamage(value, damageType);
        }
    }
}