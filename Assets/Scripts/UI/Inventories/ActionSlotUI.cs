﻿using System;
using RPG.Abilities;
using RPG.Inventories;
using UI.Dragging;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventories
{
    /// <summary>
    /// The UI slot for the player action bar.
    /// </summary>
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] int index = 0;
        [SerializeField] Image cooldownFill;

        // CACHE
        ActionStore store;
        private CooldownStore _cooldownStore;

        // LIFECYCLE METHODS
        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            store = player.GetComponent<ActionStore>();
            _cooldownStore = player.GetComponent<CooldownStore>();
            store.storeUpdated += UpdateIcon;
        }

        private void Update()
        {
            cooldownFill.fillAmount = _cooldownStore.GetFractionRemaining(GetItem());
        }

        // PUBLIC

        public void AddItems(InventoryItem item, int number)
        {
            store.AddAction(item, index, number);
            FMODUnity.RuntimeManager.PlayOneShot(item.equipSFX);
        }

        public InventoryItem GetItem()
        {
            return store.GetAction(index);
        }

        public int GetNumber()
        {
            return store.GetNumber(index);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return store.MaxAcceptable(item, index);
        }

        public void RemoveItems(int number)
        {
            store.RemoveItems(index, number);
        }

        // PRIVATE

        void UpdateIcon()
        {
            icon.SetItem(GetItem(), GetNumber());
        }
    }
}
