using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameDataSO _gameData;

    [SerializeField]
    private TMP_Text _moneyLabel;

    [SerializeField]
    private TMP_Text _upgradeMenuMoneyLabel;

    [SerializeField]
    private TMP_Text _inventoryLabel;

    [SerializeField]
    private TMP_Text _inventoryFullLabel;

    [SerializeField]
    private float _animationDuration = 0.5f;

    [SerializeField]
    private TMP_Text _trashPiecesLabel;

    [SerializeField]
    private TMP_Text _gameWonLabel;

    [SerializeField]
    private Button _quitButton;

    [SerializeField]
    private GameObject _upgradePanel;

    public Button UpgradeOpenButton;

    [SerializeField]
    public Button _upgradeCloseButton;

    [SerializeField]
    private Button _unlockRecyclingCenterButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameData.IsGamePaused = false;
        _gameData.BoatSpeedUpgradeLevel = 0;
        _gameData.BoatInventoryUpgradeLevel = 0;
        _gameData.BoatSpeed = 20f;
        _gameData.BoatInventoryCapacity = 5;
        _gameData.Money = 0;
        _gameData.BoatInventory = new List<TrashData>();
        _gameData.TrashPiecesCollected = 0;
        _gameData.TotalTrashPieces = 0;

        _moneyLabel.text = $"{_gameData.Money:C0}";
        _upgradeMenuMoneyLabel.text = $"{_gameData.Money:C0}";
        _inventoryLabel.text =
            $"{_gameData.GetBoatInventoryWeight()}/{_gameData.BoatInventoryCapacity}";

        UpgradeOpenButton.onClick.AddListener(HandleOpenUpgradePanel);
        _upgradeCloseButton.onClick.AddListener(HandleCloseUpgradePanel);

        _quitButton.onClick.AddListener(HandleQuitGame);
        _gameData.TotalTrashPieces = FindObjectsByType<TrashController>(FindObjectsSortMode.None).Length;
        _trashPiecesLabel.text = $"{_gameData.TrashPiecesCollected}/{_gameData.TotalTrashPieces}";

        _unlockRecyclingCenterButton.interactable = false;
        _unlockRecyclingCenterButton.gameObject.SetActive(false);
        _unlockRecyclingCenterButton.onClick.AddListener(HandleUnlockRecyclingCenter);
    }

    void OnEnable()
    {
        EventManager.UI.OnInventoryChanged += OnInventoryChanged;
        EventManager.UI.OnMoneyChanged += OnMoneyChanged;
        // EventManager.Game.OnDropZoneEntered += OnDropZoneEntered;
        EventManager.Game.OnDropZoneExited += OnDropZoneExited;
        EventManager.Game.OnLockedDropZoneEntered += OnLockedDropZoneEntered;
        EventManager.Game.OnLockedDropZoneExited += OnLockedDropZoneExited;
        EventManager.UI.OnInventoryFull += OnInventoryFull;
        EventManager.Game.OnGameWon += OnGameWon;

 
    }

    void OnDisable()
    {
        EventManager.UI.OnInventoryChanged -= OnInventoryChanged;
        EventManager.UI.OnMoneyChanged -= OnMoneyChanged;
        // EventManager.Game.OnDropZoneEntered -= OnDropZoneEntered;
        EventManager.Game.OnDropZoneExited -= OnDropZoneExited;
        EventManager.Game.OnLockedDropZoneEntered -= OnLockedDropZoneEntered;
        EventManager.Game.OnLockedDropZoneExited -= OnLockedDropZoneExited;
        EventManager.UI.OnInventoryFull -= OnInventoryFull;
        EventManager.Game.OnGameWon -= OnGameWon;

    }



    void OnInventoryChanged()
    {
        float currentWeight = float.Parse(_inventoryLabel.text.Split('/')[0]);
        float targetWeight = _gameData.GetBoatInventoryWeight();
        int capacity = _gameData.BoatInventoryCapacity;

        DOTween
            .To(() => currentWeight, x => currentWeight = x, targetWeight, _animationDuration)
            .OnUpdate(() => _inventoryLabel.text = $"{Mathf.Round(currentWeight)}/{capacity}")
            .SetEase(Ease.OutCubic);

        _trashPiecesLabel.text = $"{_gameData.TrashPiecesCollected}/{_gameData.TotalTrashPieces}";

        _moneyLabel.text = $"{_gameData.Money:C0}";

        _upgradeMenuMoneyLabel.text = $"{_gameData.Money:C0}";
    }

    async void OnInventoryFull()
    {
        _inventoryFullLabel.gameObject.SetActive(true);
        await Task.Delay(3000);
        _inventoryFullLabel.gameObject.SetActive(false);
    }

    void OnMoneyChanged()
    {
        float currentMoney = float.Parse(_moneyLabel.text.Substring(1).Replace(",", ""));
        float targetMoney = _gameData.Money;

        DOTween
            .To(() => currentMoney, x => currentMoney = x, targetMoney, _animationDuration)
            .OnUpdate(() => _moneyLabel.text = $"{currentMoney:C0}")
            .SetEase(Ease.OutCubic);


        _upgradeMenuMoneyLabel.text = $"{_gameData.Money:C0}";
    }

    void OnDropZoneExited()
    {
        UpgradeOpenButton.gameObject.SetActive(false);
    }

    void OnLockedDropZoneEntered(RecyclingCenterController recyclingCenter)
    {
        if (_gameData.Money < recyclingCenter.PriceToUnlock)
        {
            _unlockRecyclingCenterButton.interactable = false;
        }
        else
        {
            _unlockRecyclingCenterButton.interactable = true;
        }

        _unlockRecyclingCenterButton.gameObject.SetActive(true);
        _gameData.CurrentRecyclingCenter = recyclingCenter;
        _unlockRecyclingCenterButton.GetComponentInChildren<TMP_Text>().text =
            $"UNLOCK FOR {_gameData.CurrentRecyclingCenter.PriceToUnlock:C0}";
    }

    void OnLockedDropZoneExited()
    {
        _unlockRecyclingCenterButton.gameObject.SetActive(false);

        _gameData.CurrentRecyclingCenter = null;
    }

    public void HandleUnlockRecyclingCenter()
    {
        Debug.Log("HandleUnlockRecyclingCenter being pressed");
        Debug.Log(
            $"Money: {_gameData.Money}, PriceToUnlock: {_gameData.CurrentRecyclingCenter.PriceToUnlock}"
        );
        if (_gameData.Money >= _gameData.CurrentRecyclingCenter.PriceToUnlock)
        {
            _gameData.Money -= _gameData.CurrentRecyclingCenter.PriceToUnlock;
            _gameData.CurrentRecyclingCenter.HandleUnlockRecyclingCenter();

            _unlockRecyclingCenterButton.gameObject.SetActive(false);
            _gameData.CurrentRecyclingCenter = null;

            EventManager.Game.InvokeDropZoneEntered();
        }
    }

    public void HandleOpenUpgradePanel()
    {
        EventManager.UI.InvokePauseGame();
        _upgradePanel.SetActive(true);
    }

    public void HandleCloseUpgradePanel()
    {
        EventManager.UI.InvokeResumeGame();
        _upgradePanel.SetActive(false);
    }

    public void HandleQuitGame()
    {
        SceneManager.LoadScene("100_MainMenu");
    }

    async void OnGameWon()
    {
        _gameWonLabel.gameObject.SetActive(true);
        await Task.Delay(2000);
        SceneManager.LoadScene("300_WinMenu");
    }

    
}
