using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static readonly UIEvents UI = new UIEvents();
    public static readonly GameEvents Game = new GameEvents();

    public class UIEvents
    {
        public UnityAction OnInventoryChanged;
        public UnityAction OnSpeedChanged;
        public UnityAction OnMoneyChanged;
        public UnityAction OnPauseGame;
        public UnityAction OnResumeGame;
        public UnityAction<BoatSpeedUpgrade> OnSpeedUpgradePurchased;
        public UnityAction<BoatInventoryUpgrade> OnInventoryUpgradePurchased;
        public UnityAction OnInventoryFull;
    }

    public class GameEvents
    {
        public UnityAction<TrashController> OnTrashCollected;
        public UnityAction OnDropZoneEntered;
        public UnityAction OnDropZoneExited;
    }
}
