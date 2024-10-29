using UnityEngine;

public class RecyclingCenterController : MonoBehaviour
{
    public bool IsUnlocked = false;
    public int PriceToUnlock = 10;

    [SerializeField] public GameObject _dropZone;
    [SerializeField] private Material _lockedMaterial;
    [SerializeField] private Material _unlockedMaterial;


    void Start()
    {
        var renderer = _dropZone.GetComponent<MeshRenderer>();
        renderer.material = new Material(IsUnlocked ? _unlockedMaterial : _lockedMaterial);
    }

    public void HandleUnlockRecyclingCenter()
    {
        IsUnlocked = true;
        var renderer = _dropZone.GetComponent<MeshRenderer>();
        renderer.material = new Material(_unlockedMaterial);
    }


}
