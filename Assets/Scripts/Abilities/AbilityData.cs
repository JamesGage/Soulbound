using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    public class AbilityData
    {
        private GameObject _user;
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

        public GameObject GetUser()
        {
            return _user;
        }
    }
}