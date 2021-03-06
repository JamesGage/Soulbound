using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Utils;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "Abilities/Targeting/Delayed Click")]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] Texture2D cursorTexture;
        [SerializeField] Vector2 cursorHotspot;
        [SerializeField] LayerMask layerMask;
        [SerializeField] Transform targetingPrefab;
        [SerializeField] float areaAffectRadius;
        [SerializeField] private float maxDistance;

        private Transform targetingPrefabInstance = null;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            PlayerController playerController = data.GetUser().GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(data, playerController, finished));
        }

        private IEnumerator Targeting(AbilityData data, PlayerController playerController, Action finished)
        {
            playerController.enabled = false;
            if (targetingPrefabInstance == null)
            {
                targetingPrefabInstance = Instantiate(targetingPrefab); 
            }
            else
            {
                targetingPrefabInstance.gameObject.SetActive(true);
            }

            targetingPrefabInstance.localScale = new Vector3(areaAffectRadius * 2, areaAffectRadius * 2, areaAffectRadius * 2);
            var meshRenderer = targetingPrefabInstance.GetComponent<MeshRenderer>();

            while (!data.IsCancelled())
            {
                Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
                RaycastHit raycastHit;
                if (Physics.Raycast(PlayerController.GetMouseRay(), out raycastHit, 1000, layerMask))
                {
                    targetingPrefabInstance.position = raycastHit.point;
                    meshRenderer.enabled = !data.IsInRange(raycastHit.point, maxDistance);

                    if (Input.GetKeyDown(InputManager.inputManager.interact) && data.IsInRange(raycastHit.point, maxDistance))
                    {
                        //Absorb whole mouse click
                        yield return new WaitWhile(() => Input.GetKeyDown(InputManager.inputManager.interact));
                        
                        data.SetTargetedPoint(raycastHit.point);
                        data.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                        break;
                    }

                    if (Input.GetKeyDown(InputManager.inputManager.cancel))
                    {
                        data.Cancel();
                        break;
                    }
                }
                yield return null;
            }
            targetingPrefabInstance.gameObject.SetActive(false);
            playerController.enabled = true;
            finished();
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            var hits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.up, 0);
            foreach (var hit in hits)
            {
                yield return hit.collider.gameObject;
            }
        }
    }
}