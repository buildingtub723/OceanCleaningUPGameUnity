using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class NetController : MonoBehaviour
{
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
            if (GameManager.Instance.GetBoatInventoryWeight() + trashWeight <= GameManager.Instance.GetInventoryCapacity()) 
            {
                EventManager.Game.OnTrashCollected?.Invoke(trash);
                Destroy(other.gameObject);
            }
        }
    }
}
