using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TMP_Text _text;
        [SerializeField] Color _damageColor;

        public void SetDamage(float damage)
        {
            _text.text = $"{damage:N0}";
            _text.color = _damageColor;
        }
    }
}