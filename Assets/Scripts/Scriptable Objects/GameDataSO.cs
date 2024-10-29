using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData", order = 0)]

public class GameDataSO : ScriptableObject
{
    public bool IsGamePaused = false;

    public int TrashPiecesCollected = 0;
    public int TotalTrashPieces = 0;

    public List<TrashData> BoatInventory = new List<TrashData>();
    public int Money = 0;
    public int BoatInventoryUpgradeLevel = 0;
    public int BoatSpeedUpgradeLevel = 0;
    public int BoatInventoryCapacity = 5;
    public float BoatSpeed = 15f;

    public RecyclingCenterController CurrentRecyclingCenter;

    void OnEnable()
    {
        EventManager.Game.OnTrashCollected += OnTrashCollected;
        EventManager.Game.OnDropZoneEntered += OnDropZoneEntered;
        EventManager.UI.OnPauseGame += OnPauseGame;
        EventManager.UI.OnResumeGame += OnResumeGame;
        EventManager.UI.OnSpeedUpgradePurchased += OnSpeedUpgradePurchased;
        EventManager.UI.OnInventoryUpgradePurchased += OnInventoryUpgradePurchased;
    }

    void OnDisable()
    {
        EventManager.Game.OnTrashCollected -= OnTrashCollected;
        EventManager.Game.OnDropZoneEntered -= OnDropZoneEntered;
        EventManager.UI.OnPauseGame -= OnPauseGame;
        EventManager.UI.OnResumeGame -= OnResumeGame;
        EventManager.UI.OnSpeedUpgradePurchased -= OnSpeedUpgradePurchased;
        EventManager.UI.OnInventoryUpgradePurchased -= OnInventoryUpgradePurchased;
    }

    void OnTrashCollected(TrashController trash)
    {
        BoatInventory.Add(trash.TrashData);
        EventManager.UI.OnInventoryChanged?.Invoke();
    }

    void OnDropZoneEntered()
    {
        Debug.Log("Drop zone entered");
        foreach (TrashData trash in BoatInventory)
        {
            Money += trash.Value;
            TrashPiecesCollected++;
        }
        BoatInventory.Clear();

        EventManager.UI.OnInventoryChanged?.Invoke();
        EventManager.UI.OnMoneyChanged?.Invoke();

        if (TotalTrashPieces == TrashPiecesCollected)
        {
            EventManager.Game.OnGameWon?.Invoke();
        }
    }

    void OnPauseGame()
    {
        IsGamePaused = true;
    }

    void OnResumeGame()
    {
        IsGamePaused = false;
    }

    public int GetBoatInventoryWeight()
    {
        int inventoryWeight = 0;
        foreach (TrashData trash in BoatInventory)
        {
            inventoryWeight += trash.InventoryWeight;
        }
        return inventoryWeight;
    }

    void OnSpeedUpgradePurchased(BoatSpeedUpgrade upgrade)
    {
        BoatSpeedUpgradeLevel = upgrade.Level;
        BoatSpeed = upgrade.Speed;
        Money -= upgrade.Price;
        EventManager.UI.OnSpeedChanged?.Invoke();
        EventManager.UI.OnMoneyChanged?.Invoke();
    }

    void OnInventoryUpgradePurchased(BoatInventoryUpgrade upgrade)
    {
        BoatInventoryUpgradeLevel = upgrade.Level;
        BoatInventoryCapacity = upgrade.Slots;
        Money -= upgrade.Price;
        EventManager.UI.OnInventoryChanged?.Invoke();
        EventManager.UI.OnMoneyChanged?.Invoke();
    }

}
