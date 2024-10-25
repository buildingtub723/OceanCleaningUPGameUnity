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
        public UnityAction OnMoneyChanged;
    }

    public class GameEvents
    {
        public UnityAction<TrashController> OnTrashCollected;
        public UnityAction OnDropZoneEntered;
    }
}
