using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Abilities;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;

namespace RPG.Inventories
{
    public class ActionStore : MonoBehaviour, ISaveable
    {
        [SerializeField] private Transform _queueOpenPrefab;
        
        Dictionary<int, Ability> dockedItems = new Dictionary<int, Ability>();
        private GameObject _user;
        private Ability _ability;
        private CooldownStore _cooldownStore;
        private bool _inUse;
        private bool _isOpen;

        public event Action storeUpdated;
        
        public Ability GetAction(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index];
            }
            return null;
        }

        public void AddAction(Ability ability, int index)
        {
            dockedItems[index] = ability;

            storeUpdated?.Invoke();
        }
        
        public void RemoveItems(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems.Remove(index);
                storeUpdated?.Invoke();
            }
        }
        
        public IEnumerator Use(int index, GameObject user)
        {
            if (_isOpen)
            {
                ActivateAbility(index, user);
            }
            if(_inUse) yield break;
            
            ActivateAbility(index, user);
        }

        private void ActivateAbility(int index, GameObject user)
        {
            _user = user;
            var health = _user.GetComponent<Health>();
            
            if (!dockedItems.ContainsKey(index)) return;
            if(health.IsDead()) return;

            _ability = dockedItems[index];
            _cooldownStore = user.GetComponent<CooldownStore>();
            if (_cooldownStore.GetTimeRemaining(_ability) > 0) return;
            
            StartCoroutine(InUse());

            _ability.Use(user);
        }

        private IEnumerator InUse()
        {
            _inUse = true;
            yield return new WaitForSeconds(_ability.GetQueueReadyTime());
            _isOpen = true;
            OpenQueue(_ability.GetQueueOpenTime());
            yield return new WaitForSeconds(_ability.GetQueueOpenTime());
            _isOpen = false;
            _inUse = false;
        }

        private void OpenQueue(float time)
        {
            var queue = Instantiate(_queueOpenPrefab, _user.transform);
            StartCoroutine(QueueScale(time, queue));
        }

        private IEnumerator QueueScale(float time, Transform queue)
        {
            queue.localScale = new Vector3(10, 10, 10);
            var elapsedTime = 0f;
            while (elapsedTime < time)
            {
                queue.localScale = Vector3.Lerp (queue.localScale, Vector3.up, elapsedTime/time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Destroy(queue.gameObject);
        }

        object ISaveable.CaptureState()
        {
            var state = new Dictionary<int, string>();
            foreach (var ability in dockedItems)
            {
                state[ability.Key] = ability.Value.GetItemID();
            }
            return state;
        }

        void ISaveable.RestoreState(object state)
        {
            var stateDict = (Dictionary<int, string>)state;
            foreach (var ability in stateDict)
            {
                AddAction(InventoryItem.GetFromID(ability.Value) as Ability, ability.Key);
            }
        }
    }
}