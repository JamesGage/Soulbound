using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour, IModifierProvider
    {
        #region Variables

        [SerializeField] float chaseDistance = 5f;
        [Range(0f, 5f)]
        [SerializeField] float suspicionTime = 2f;
        [SerializeField] float _aggroCooldownTime = 3f;
        [SerializeField] float _shoutDistance = 5f;

        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 1f;
        [Range(0f, 1f)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        private Mover _mover;
        private Fighter _fighter;
        private GameObject _player;
        private Health _health;
        private ActionScheduler _actionScheduler;

        LazyValue<Vector3> _guardPosition;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private float _timeSinceAggrevated = Mathf.Infinity;
        private bool _canShout = true;
        private int _currentWaypointIndex = 0;

        #endregion

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _player = GameObject.FindWithTag("Player");
            
            _guardPosition = new LazyValue<Vector3>(GetGuardPosition);
            _guardPosition.ForceInit();
        }

        private void Update()
        {
            if(_health.IsDead()) return;
            
            if (IsAggrevated() && _fighter.CanAttack(_player))
            {
                AttackBehavior();
            }
            else if (_timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }

            UpdateTimers();
        }

        public void Aggrevate()
        {
            _timeSinceAggrevated = 0;
        }
        
        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        #region Attack

        private void AttackBehavior()
        {
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(_player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            if (_canShout)
            {
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, _shoutDistance, Vector3.up, 0);
                foreach (var hit in hits)
                {
                    var ai = hit.transform.GetComponent<AIController>();
                    if(ai == null) continue;
                    if (ai == this) continue;
                
                    ai.Aggrevate();
                }
            }
            _canShout = false;
        }

        private bool IsAggrevated()
        {
            var distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || _timeSinceAggrevated < _aggroCooldownTime;
        }

        #endregion
        
        #region Patrol

        private void PatrolBehavior()
        {
            Vector3 nextPosition = _guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    _timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (_timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                _mover.StartMoveAction(nextPosition, patrolSpeedFraction);   
            }
            
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            _currentWaypointIndex = patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(_currentWaypointIndex);
        }

        #endregion
        
        private void SuspicionBehavior()
        {
            _actionScheduler.CancelCurrentAction();
            _canShout = true;
        }
        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
            _timeSinceAggrevated += Time.deltaTime;
        }
        
        public void Reset()
        {
            var navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.Warp(_guardPosition.value);
            
            _timeSinceLastSawPlayer = Mathf.Infinity;
            _timeSinceArrivedAtWaypoint = Mathf.Infinity;
            _timeSinceAggrevated = Mathf.Infinity;
            _currentWaypointIndex = 0;
            _canShout = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        public IEnumerable<float> GetAddativeModifiers(Stat stat)
        {
            return _fighter.GetCurrentWeapon().GetAddativeModifiers(stat);
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            return _fighter.GetCurrentWeapon().GetPercentageModifiers(stat);
        }
    }
}