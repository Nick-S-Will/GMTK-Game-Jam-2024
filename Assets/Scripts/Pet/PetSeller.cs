using UnityEngine;
using UnityEngine.Events;

public class PetSeller : MonoBehaviour, IDropPoint<Pet>
{
    [Header("Events")]
    public UnityEvent<Pet> OnSell;
    [Header("Debug")]
    [SerializeField] private bool logEvents;

    private void Awake()
    {
        if (logEvents)
        {
            OnSell.AddListener(pet => Debug.Log(nameof(OnSell) + ": " + pet.name));
        }
    }

    public bool TryPlace(Pet pet)
    {
        if (pet == null) return false;

        OnSell.Invoke(pet);

        return true;
    }

    public Pet TryRemove() => null;
}