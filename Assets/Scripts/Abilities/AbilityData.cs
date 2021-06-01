using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    public class AbilityData
    {
        private GameObject _user;
        private Vector3 _targetedPoint;
        private IEnumerable<GameObject> _targets;


        public AbilityData(GameObject user)
        {
            this._user = user;
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

        public void StartCoroutine(IEnumerator coroutine)
        {
            _user.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
        }
    }
}