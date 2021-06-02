﻿using RPG.Combat;
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

        public void SetDamage(float damage, DamageType damageType, bool isCritical, WeaponConfig weapon)
        {
            if (damageType == DamageType.Healing)
            {
                _text.text = $"{damage:N0}";
                _text.color = _healColor;
                //FMODUnity.RuntimeManager.PlayOneShot(healing, transform.position);
                return;
            }
            if (isCritical)
            {
                _text.text = $"{damage:N0}";
                _text.color = _criticalColor;
                FMODUnity.RuntimeManager.PlayOneShot(weapon.critSFX, transform.position);
                return;
            }
            if (damageType == DamageType.Block)
            {
                _text.text = $"{damage:N0}";
                _text.color = _blockColor;
                FMODUnity.RuntimeManager.PlayOneShot(weapon.blockSFX, transform.position);
                return;
            }
            if (damage >= 0)
            {
                _text.text = $"{damage:N0}";
                _text.color = _damageColor;
                FMODUnity.RuntimeManager.PlayOneShot(weapon.hitSFX, transform.position);
                return;
            }
            
            if (damageType == DamageType.Miss)
            {
                _text.text = _missText;
                _text.color = _missColor;
                FMODUnity.RuntimeManager.PlayOneShot(weapon.missSFX, transform.position);
            }
        }
    }
}