using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class NetController : MonoBehaviour
{
    [SerializeField] private GameDataSO _gameData;
    private Rigidbody _rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            TrashController trash = other.GetComponent<TrashController>();
            int trashWeight = trash.TrashData.InventoryWeight;
            if (_gameData.GetBoatInventoryWeight() + trashWeight <= _gameData.BoatInventoryCapacity) 
            {
                EventManager.Game.OnTrashCollected?.Invoke(trash);
                Destroy(other.gameObject);
            }
            else
            {
                EventManager.UI.OnInventoryFull?.Invoke();
            }
        }
    }
}
