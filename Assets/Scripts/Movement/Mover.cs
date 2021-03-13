using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] private float maxNavPathLength = 25f;
        
        [FMODUnity.EventRef] public string footstepSFX = "";
        
        private NavMeshAgent _navAgent;
        private Animator _anim;
        private ActionScheduler _actionScheduler;
        private Health _health;
        private Ground.GroundType _groundType;

        private void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _navAgent.enabled = !_health.IsDead();
            
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            var velocity = _navAgent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;
            _anim.SetFloat("forwardSpeed", speed);
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;
            
            return true;
        }
        
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _navAgent.destination = destination;
            _navAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            _navAgent.isStopped = false;
        }

        public void Cancel()
        {
            _navAgent.isStopped = true;
        }

        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }
        
        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData) state;
            _navAgent.Warp(data.position.ToVector());
            transform.eulerAngles = data.rotation.ToVector();
        }
        
        //Animation Event
        public void Footstep()
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit);
            if (hit.transform.GetComponent<Ground>() == null) return;
            
            var ground = hit.transform.GetComponent<Ground>();
            PlayGroundType(ground);
        }
        
        private float  GetPathLength(NavMeshPath path)
        {
            float total = 0f;
            if (path.corners.Length < 2) return total;

            for (int i = 0; i < path.corners.Length - 1; i++)
            { 
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            
            return total;
        }

        private void PlayGroundType(Ground ground)
        {
            FMOD.Studio.EventInstance footstep = FMODUnity.RuntimeManager.CreateInstance(footstepSFX);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(footstep, transform, GetComponent<Rigidbody>());
            footstep.setParameterByName("Material", (int)ground.groundType);
            footstep.setParameterByName("Speed", 0);
            footstep.start();
            footstep.release();
        }
    }
}
