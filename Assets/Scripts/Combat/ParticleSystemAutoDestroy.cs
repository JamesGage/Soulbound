using UnityEngine;

namespace RPG.Combat
{
    public class ParticleSystemAutoDestroy : MonoBehaviour
    {
        private ParticleSystem ps;
        
        public void Awake() 
        {
            ps = GetComponent<ParticleSystem>();
        }
 
        public void Update() 
        {
            if(ps)
            {
                if(!ps.IsAlive())
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}