using RPG.Core;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(BaseStats), typeof(Animator))]
    public class PlayerFighter : MonoBehaviour, IAction
    {
        private BaseStats _baseStats;
        private Animator _anim;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private ActionScheduler _actionScheduler;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _baseStats = GetComponent<BaseStats>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
        }

        public void Attack()
        {
            _actionScheduler.StartAction(this);
            
            if (_timeSinceLastAttack > _baseStats.GetStat(Stat.Speed) + .1f)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            _anim.ResetTrigger("stopAttack");
            _anim.SetTrigger("attack");
            _anim.speed = _baseStats.GetStat(Stat.Speed);
        }
        
        private void StopAttack()
        {
            _anim.ResetTrigger("attack");
            _anim.SetTrigger("stopAttack");
        }

        public void Cancel()
        {
            StopAttack();
        }
    }
}