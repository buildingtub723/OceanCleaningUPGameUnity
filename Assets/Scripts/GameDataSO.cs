using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData", order = 0)]

public class GameDataSO : ScriptableObject
{

    public List<BoatInventoryUpgrade> BoatInventoryUpgrades;
    public List<BoatSpeedUpgrade> BoatSpeedUpgrades;

    public List<TrashData> BoatInventory = new List<TrashData>();
    public int Money = 0;
    public int BoatInventoryUpgradeIndex = -1;
    public int BoatSpeedUpgradeIndex = -1;
    public int BoatInventoryCapacity = 5;
    public float BoatSpeed = 10f;

    void OnEnable()
    {
        EventManager.Game.OnTrashCollected += OnTrashCollected;
        EventManager.Game.OnDropZoneEntered += OnDropZoneEntered;
    }

    void OnDisable()
    {
        EventManager.Game.OnTrashCollected -= OnTrashCollected;
        EventManager.Game.OnDropZoneEntered -= OnDropZoneEntered;
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
        }
        BoatInventory.Clear();

        EventManager.UI.OnInventoryChanged?.Invoke();
        EventManager.UI.OnMoneyChanged?.Invoke();
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

}
