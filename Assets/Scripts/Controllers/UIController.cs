using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
public class UIController : MonoBehaviour
{
    [SerializeField] private GameDataSO _gameData;
    [SerializeField] private TMP_Text _moneyLabel;
    [SerializeField] private TMP_Text _upgradeMenuMoneyLabel;
    [SerializeField] private TMP_Text _inventoryLabel;
    [SerializeField] private TMP_Text _inventoryFullLabel;
    [SerializeField] private float _animationDuration = 0.5f;

    [SerializeField] private Button _quitButton;

    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private Button _upgradeOpenButton;
    [SerializeField] private Button _upgradeCloseButton;


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

        _moneyLabel.text = $"{_gameData.Money:C0}";
        _upgradeMenuMoneyLabel.text = $"{_gameData.Money:C0}";
        _inventoryLabel.text = $"{_gameData.GetBoatInventoryWeight()}/{_gameData.BoatInventoryCapacity}";

        _upgradeOpenButton.onClick.AddListener(HandleOpenUpgradePanel);
        _upgradeCloseButton.onClick.AddListener(HandleCloseUpgradePanel);

        _quitButton.onClick.AddListener(HandleQuitGame);
    }

    void OnEnable()
    {
        EventManager.UI.OnInventoryChanged += OnInventoryChanged;
        EventManager.UI.OnMoneyChanged += OnMoneyChanged;
        EventManager.Game.OnDropZoneEntered += OnDropZoneEntered;
        EventManager.Game.OnDropZoneExited += OnDropZoneExited;
        EventManager.UI.OnInventoryFull += OnInventoryFull;
    }

    void OnDisable()
    {
        EventManager.UI.OnInventoryChanged -= OnInventoryChanged;
        EventManager.UI.OnMoneyChanged -= OnMoneyChanged;
        EventManager.Game.OnDropZoneEntered -= OnDropZoneEntered;
        EventManager.Game.OnDropZoneExited -= OnDropZoneExited;
        EventManager.UI.OnInventoryFull -= OnInventoryFull;
    }

    void OnInventoryChanged()
    {
        float currentWeight = float.Parse(_inventoryLabel.text.Split('/')[0]);
        float targetWeight = _gameData.GetBoatInventoryWeight();
        int capacity = _gameData.BoatInventoryCapacity;

        DOTween.To(() => currentWeight, x => currentWeight = x, targetWeight, _animationDuration)
            .OnUpdate(() => _inventoryLabel.text = $"{Mathf.Round(currentWeight)}/{capacity}")
            .SetEase(Ease.OutCubic);
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

        DOTween.To(() => currentMoney, x => currentMoney = x, targetMoney, _animationDuration)
            .OnUpdate(() => _moneyLabel.text = $"{currentMoney:C0}")
            .SetEase(Ease.OutCubic);

        _upgradeMenuMoneyLabel.text = $"{_gameData.Money:C0}";
    }

    void OnDropZoneEntered()
    {
        _upgradeOpenButton.gameObject.SetActive(true);
    }

    void OnDropZoneExited()
    {
        _upgradeOpenButton.gameObject.SetActive(false);
    }

    public void HandleOpenUpgradePanel()
    {
        EventManager.UI.OnPauseGame?.Invoke();
        _upgradePanel.SetActive(true);
    }

    public void HandleCloseUpgradePanel()
    {
        EventManager.UI.OnResumeGame?.Invoke();
        _upgradePanel.SetActive(false);
    }

    public void HandleQuitGame()
    {
        SceneManager.LoadScene("100_MainMenu");
    }
}
