using RPG.Combat;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TMP_Text _text;
        [SerializeField] string _missText = "MISS";
        [SerializeField] Color _missColor;
        [SerializeField] Color _damageColor;
        [SerializeField] Color _blockColor;
        [SerializeField] Color _criticalColor;
        [SerializeField] Color _healColor;

        public void SetDamage(float damage, DamageType damageType)
        {
            if (damageType == DamageType.Healing)
            {
                _text.text = $"{damage:N0}";
                _text.color = _healColor;
                return;
            }
            if (damageType == DamageType.Block)
            {
                _text.text = $"{damage:N0}";
                _text.color = _blockColor;
                return;
            }
            if (damage >= 0)
            {
                _text.text = $"{damage:N0}";
                _text.color = _damageColor;
                return;
            }
            
            if (damageType == DamageType.Miss)
            {
                _text.text = _missText;
                _text.color = _missColor;
            }
        }
    }
}