using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventManager
{
    public static readonly UIEvents UI = new UIEvents();
    public static readonly GameEvents Game = new GameEvents();
    public static readonly HackEvents Hack = new HackEvents();
    public class UIEvents
    {
        private event Action _onInventoryChanged;
        private event Action _onSpeedChanged;
        private event Action _onMoneyChanged;
        private event Action _onPauseGame;
        private event Action _onResumeGame;
        private event Action<BoatSpeedUpgrade> _onSpeedUpgradePurchased;
        private event Action<BoatInventoryUpgrade> _onInventoryUpgradePurchased;
        private event Action _onInventoryFull;

        // Invoke methods
        public void InvokeInventoryChanged() => _onInventoryChanged?.Invoke();
        public void InvokeSpeedChanged() => _onSpeedChanged?.Invoke();
        public void InvokeMoneyChanged() => _onMoneyChanged?.Invoke();
        public void InvokePauseGame() => _onPauseGame?.Invoke();
        public void InvokeResumeGame() => _onResumeGame?.Invoke();
        public void InvokeSpeedUpgradePurchased(BoatSpeedUpgrade upgrade) => _onSpeedUpgradePurchased?.Invoke(upgrade);
        public void InvokeInventoryUpgradePurchased(BoatInventoryUpgrade upgrade) => _onInventoryUpgradePurchased?.Invoke(upgrade);
        public void InvokeInventoryFull() => _onInventoryFull?.Invoke();

        // Public event properties
        public event Action OnInventoryChanged
        {
            add => _onInventoryChanged += value;
            remove => _onInventoryChanged -= value;
        }
        public event Action OnSpeedChanged
        {
            add => _onSpeedChanged += value;
            remove => _onSpeedChanged -= value;
        }
        public event Action OnMoneyChanged
        {
            add => _onMoneyChanged += value;
            remove => _onMoneyChanged -= value;
        }
        public event Action OnPauseGame
        {
            add => _onPauseGame += value;
            remove => _onPauseGame -= value;
        }
        public event Action OnResumeGame
        {
            add => _onResumeGame += value;
            remove => _onResumeGame -= value;
        }
        public event Action<BoatSpeedUpgrade> OnSpeedUpgradePurchased
        {
            add => _onSpeedUpgradePurchased += value;
            remove => _onSpeedUpgradePurchased -= value;
        }
        public event Action<BoatInventoryUpgrade> OnInventoryUpgradePurchased
        {
            add => _onInventoryUpgradePurchased += value;
            remove => _onInventoryUpgradePurchased -= value;
        }
        public event Action OnInventoryFull
        {
            add => _onInventoryFull += value;
            remove => _onInventoryFull -= value;
        }
    }

    public class GameEvents
    {
        private event Action<TrashController> _onTrashCollected;
        private event Action _onDropZoneEntered;
        private event Action _onDropZoneExited;
        private event Action<RecyclingCenterController> _onLockedDropZoneEntered;
        private event Action _onLockedDropZoneExited;
        private event Action _onGameWon;

        // Invoke methods
        public void InvokeTrashCollected(TrashController trash) => _onTrashCollected?.Invoke(trash);
        public void InvokeDropZoneEntered() => _onDropZoneEntered?.Invoke();
        public void InvokeDropZoneExited() => _onDropZoneExited?.Invoke();
        public void InvokeLockedDropZoneEntered(RecyclingCenterController center) => _onLockedDropZoneEntered?.Invoke(center);
        public void InvokeLockedDropZoneExited() => _onLockedDropZoneExited?.Invoke();
        public void InvokeGameWon() => _onGameWon?.Invoke();

        // Public event properties
        public event Action<TrashController> OnTrashCollected
        {
            add => _onTrashCollected += value;
            remove => _onTrashCollected -= value;
        }
        public event Action OnDropZoneEntered
        {
            add => _onDropZoneEntered += value;
            remove => _onDropZoneEntered -= value;
        }
        public event Action OnDropZoneExited
        {
            add => _onDropZoneExited += value;
            remove => _onDropZoneExited -= value;
        }
        public event Action<RecyclingCenterController> OnLockedDropZoneEntered
        {
            add => _onLockedDropZoneEntered += value;
            remove => _onLockedDropZoneEntered -= value;
        }
        public event Action OnLockedDropZoneExited
        {
            add => _onLockedDropZoneExited += value;
            remove => _onLockedDropZoneExited -= value;
        }
        public event Action OnGameWon
        {
            add => _onGameWon += value;
            remove => _onGameWon -= value;
        }
    }

    public class HackEvents
    {
        private event Action _onOpenUpgradeMenu;

        public event Action OnOpenUpgradeMenu
        {
            add => _onOpenUpgradeMenu += value;
            remove => _onOpenUpgradeMenu -= value;
        }

        public void InvokeOpenUpgradeMenu() => _onOpenUpgradeMenu?.Invoke();

    }
}
