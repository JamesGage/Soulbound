using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Fighter), typeof(Health))]
    public class AutoAttack : MonoBehaviour
    {
        private Fighter _fighter;
        private Health _health;
        
        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            _health.onHealthChanged += FindTarget;
        }

        private void FindTarget()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _fighter.GetCurrentWeapon().Range(), Vector3.up, 0);
            foreach (var hit in hits)
            {
                var enemy = hit.transform.GetComponent<AIController>();
                if(enemy == null) continue;

                if(!_fighter.CanAttack(enemy.gameObject)) continue;
                
                _fighter.Attack(enemy.gameObject);
                return;
            }
        }
    }
}