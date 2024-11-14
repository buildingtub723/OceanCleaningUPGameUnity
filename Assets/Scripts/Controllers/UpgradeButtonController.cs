using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButtonController : MonoBehaviour
{
    [SerializeField] private GameDataSO _gameData;
    [SerializeField] private TMP_Text _upgradeLabel;
    [SerializeField] private TMP_Text _priceLabel;
    [SerializeField] private Button _purchaseButton;

    [SerializeField] private string _upgradeType;
    [SerializeField] private int _upgradeLevel;
    [SerializeField] private int _price;
    [SerializeField] private int _upgradeAmount;

    void Start()
    {
        _upgradeLabel.text = $"Upgrade {_upgradeLevel}";
        _priceLabel.text = _price.ToString();
        _purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
        CheckAvailability();
    }

    void OnEnable()
    {
        EventManager.UI.OnSpeedChanged += CheckAvailability;
        EventManager.UI.OnInventoryChanged += CheckAvailability;
    }

    void OnDisable()
    {
        EventManager.UI.OnSpeedChanged -= CheckAvailability;
        EventManager.UI.OnInventoryChanged -= CheckAvailability;
    }

    void CheckAvailability()
    {
        switch (_upgradeType)
        {
            case "speed":
                if (_gameData.BoatSpeedUpgradeLevel >= _upgradeLevel)
                {
                    _purchaseButton.interactable = false;
                }
                break;
            case "inventory":
                if (_gameData.BoatInventoryUpgradeLevel >= _upgradeLevel)
                {
                    _purchaseButton.interactable = false;
                }
                break;
        }
    }

    void OnPurchaseButtonClicked()
    {
        if (_gameData.Money >= _price)
        {
            switch (_upgradeType)
            {
                case "speed":
                    EventManager.UI.InvokeSpeedUpgradePurchased(new BoatSpeedUpgrade { Level = _upgradeLevel, Speed = _upgradeAmount, Price = _price });
                    break;
                case "inventory":
                    EventManager.UI.InvokeInventoryUpgradePurchased(new BoatInventoryUpgrade { Level = _upgradeLevel, Slots = _upgradeAmount, Price = _price });
                    break;
            }
            CheckAvailability();
        }
        Debug.Log("Purchase button clicked");
    }
}
