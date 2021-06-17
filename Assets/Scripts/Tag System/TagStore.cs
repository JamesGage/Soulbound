using System;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace Tag_System
{
    public class TagStore : MonoBehaviour, ISaveable
    {
        [SerializeField] private Tags tags;
        
        private Dictionary<string, bool> currentTags = new Dictionary<string, bool>();

        private void Awake()
        {
            foreach (var tag in tags.GetTags())
            {
                if(currentTags.ContainsKey(tag)) continue;
                
                currentTags.Add(tag, false);
            }
        }

        public void SetTag(string tag, bool tagState)
        {
            currentTags[tag] = tagState;
        }

        public object CaptureState()
        {
            return currentTags;
        }

        public void RestoreState(object state)
        {
            currentTags = (Dictionary<string, bool>) state;
        }
    }
}