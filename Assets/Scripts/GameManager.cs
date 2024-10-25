using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Static instance of GameManager which allows it to be accessed by any other script
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<BoatInventoryUpgrade> _boatInventoryUpgrades;
    [SerializeField] private List<BoatSpeedUpgrade> _boatSpeedUpgrades;
    private List<TrashData> _boatInventory = new List<TrashData>();
    private int _money = 0;
    private int _boatInventoryUpgradeIndex = -1;
    private int _boatSpeedUpgradeIndex = -1;
    private int _boatInventoryCapacity = 5;
    private float _boatSpeed = 10f;

    [SerializeField] private CinemachineCamera _initialCamera;
    [SerializeField] private CinemachineCamera _followCamera;

    public GameObject TitlePanel;
    public GameObject GameplayPanel;

    public bool _isGameplay = false;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

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


    void Start()
    {
    
    }

    void OnTrashCollected(TrashController trash)
    {
        _boatInventory.Add(trash.TrashData);
        EventManager.UI.OnInventoryChanged?.Invoke();
    }

    void OnDropZoneEntered()
    {
        Debug.Log("Drop zone entered");
        foreach (TrashData trash in _boatInventory)
        {
            _money += trash.Value;
        }
        _boatInventory.Clear();

        EventManager.UI.OnInventoryChanged?.Invoke();
        EventManager.UI.OnMoneyChanged?.Invoke();
    }

    public void ResetScene()
    {
        _isGameplay = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public async void InitGameplay()
    {
        _initialCamera.Priority = 0;
        _followCamera.Priority = 10;

        TitlePanel.SetActive(false);
        GameplayPanel.SetActive(true);

        await Task.Delay(1000);
        _isGameplay = true;
    }


    public int GetMoney()
    {
        return _money;
    }

    public int GetInventoryCapacity()
    {
        return _boatInventoryCapacity;
    }

    public int GetBoatInventoryWeight()
    {
        int inventoryWeight = 0;
        foreach (TrashData trash in _boatInventory)
        {
            inventoryWeight += trash.InventoryWeight;
        }
        return inventoryWeight;
    }


}

[System.Serializable]
public struct BoatInventoryUpgrade
{
    public int Slots;
    public int Cost;
}

[System.Serializable]
public struct BoatSpeedUpgrade
{
    public float Speed;
    public int Cost;
}
