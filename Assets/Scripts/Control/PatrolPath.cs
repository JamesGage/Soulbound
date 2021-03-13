using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private Color _waypointColor = Color.gray;
        
        private const float _waypointRaidus = 0.3f;
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _waypointColor;
            
            for (int i = 0; i < transform.childCount; i++)
            {
                var j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), _waypointRaidus);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
                return 0;
            return i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}