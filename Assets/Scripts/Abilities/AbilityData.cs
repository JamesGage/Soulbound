using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Abilities
{
    public class AbilityData : IAction
    {
        private GameObject _user;
        private Vector3 _targetedPoint;
        private IEnumerable<GameObject> _targets;
        private bool _cancelled = false;


        public AbilityData(GameObject user)
        {
            _user = user;
        }

        public IEnumerable<GameObject> GetTargets()
        {
            return _targets;
        }

        public void SetTargets(IEnumerable<GameObject> targets)
        {
            _targets = targets;
        }

        public void SetTargetedPoint(Vector3 targetedPoint)
        {
            _targetedPoint = targetedPoint;
        }

        public Vector3 GetTargetPoint()
        {
            return _targetedPoint;
        }

        public GameObject GetUser()
        {
            return _user;
        }

        public bool IsInRange(Vector3 currentMousePosition, float maxDistance)
        {
            return Vector3.Distance(currentMousePosition, _user.transform.position) < maxDistance;
        }

        public void StartCoroutine(IEnumerator coroutine)
        {
            _user.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
        }

        public void Cancel()
        {
            _cancelled = true;
        }

        public bool IsCancelled()
        {
            return _cancelled;
        }
    }
}