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
        [SerializeField] private Transform _stalledPrefab;
        
        Dictionary<int, Ability> dockedItems = new Dictionary<int, Ability>();
        private GameObject _user;
        private Ability _ability;
        private CooldownStore _cooldownStore;
        private Transform _stalledTempTransform;
        private AbilityState _abilityState = AbilityState.Idle;
        
        enum AbilityState
        {
            Idle,
            Started,
            QueueOpen,
            Stall
        }

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
            _ability = dockedItems[index];
            
            switch (_abilityState)
            {
                case AbilityState.Idle:
                    ActivateAbility(index, user);
                    break;
                case AbilityState.Started:
                    print(_abilityState);
                    break;
                case AbilityState.QueueOpen:
                    StopCoroutine(AbilityInUse());
                    ActivateAbility(index, user);
                    break;
                case AbilityState.Stall:
                    Stalled();
                    break;
            }
            yield break;
        }

        private void ActivateAbility(int index, GameObject user)
        {
            _user = user;
            var health = _user.GetComponent<Health>();
            
            if (!dockedItems.ContainsKey(index)) return;
            if(health.IsDead()) return;

            _cooldownStore = user.GetComponent<CooldownStore>();
            if (_cooldownStore.GetTimeRemaining(_ability) > 0) return;

            StartCoroutine(AbilityInUse());
            _ability.Use(user);
        }

        private IEnumerator AbilityInUse()
        {
            _abilityState = AbilityState.Started;
            
            if (_ability.GetHasQueue())
            {
                _abilityState = AbilityState.Stall;
                yield return new WaitForSeconds(_ability.GetQueueReadyTime());
                _abilityState = AbilityState.QueueOpen;
                OpenQueue(_ability.GetQueueOpenTime());
                yield return new WaitForSeconds(_ability.GetQueueOpenTime());
                _abilityState = AbilityState.Stall;   
            }
            yield return new WaitForSeconds(_ability.GetFinishTime());
            _abilityState = AbilityState.Idle;
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

        private void Stalled()
        {
            if (_stalledTempTransform == null)
            {
                _stalledTempTransform = Instantiate(_stalledPrefab, _user.transform);
                Destroy(_stalledTempTransform.gameObject, 1);
            }
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