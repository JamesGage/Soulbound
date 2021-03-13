using RPG.Combat;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class TargetHealthDisplay : MonoBehaviour
    {
        private Fighter _target;
        private TMP_Text _targetText;

        private void Awake()
        {
            _target = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            _targetText = gameObject.GetComponent<TMP_Text>();
        }

        private void Update()
        {
            if (_target.GetTarget() == null)
            {
                _targetText.text = "";
                return;
            }
            _targetText.text = "Target Health: " + _target.GetTarget().GetHealth() + "/" + _target.GetTarget().MaxHealth();
        }
    }
}