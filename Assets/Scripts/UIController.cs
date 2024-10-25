using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyLabel;
    [SerializeField] private TMP_Text _inventoryLabel;
    [SerializeField] private float _animationDuration = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moneyLabel.text = $"{GameManager.Instance.GetMoney():C0}";
        _inventoryLabel.text = $"{GameManager.Instance.GetBoatInventoryWeight()}/{GameManager.Instance.GetInventoryCapacity()}";
    }

    void OnEnable()
    {
        EventManager.UI.OnInventoryChanged += OnInventoryChanged;
        EventManager.UI.OnMoneyChanged += OnMoneyChanged;
    }

    void OnDisable()
    {
        EventManager.UI.OnInventoryChanged -= OnInventoryChanged;
        EventManager.UI.OnMoneyChanged -= OnMoneyChanged;
    }

    void OnInventoryChanged()
    {
        float currentWeight = float.Parse(_inventoryLabel.text.Split('/')[0]);
        float targetWeight = GameManager.Instance.GetBoatInventoryWeight();
        int capacity = GameManager.Instance.GetInventoryCapacity();

        DOTween.To(() => currentWeight, x => currentWeight = x, targetWeight, _animationDuration)
            .OnUpdate(() => _inventoryLabel.text = $"{Mathf.Round(currentWeight)}/{capacity}")
            .SetEase(Ease.OutCubic);
    }

    void OnMoneyChanged()
    {
        float currentMoney = float.Parse(_moneyLabel.text.Substring(1).Replace(",", ""));
        float targetMoney = GameManager.Instance.GetMoney();

        DOTween.To(() => currentMoney, x => currentMoney = x, targetMoney, _animationDuration)
            .OnUpdate(() => _moneyLabel.text = $"{currentMoney:C0}")
            .SetEase(Ease.OutCubic);
    }
}
