using RPG.Control;
using RPG.Inventories;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;

namespace RPG.Combat
{
   [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!enabled) return false;
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            }
            
            if (Input.GetKeyDown(InputManager.inputManager.baseAttack))
            {
                if (callingController.GetComponent<Fighter>().IsInRange(transform))
                {
                    StartCoroutine(callingController.GetComponent<ActionStore>().Use(0, callingController.gameObject));
                }
            }
            return true;
        }
    }
}