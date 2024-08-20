using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class PetSeller : MonoBehaviour, IDropPoint<Pet>
{
    [SerializeField] private OrderList orderList;
    [Header("Events")]
    public UnityEvent<Pet> OnSell;
    [Header("Debug")]
    [SerializeField] private bool logEvents;

    private void Awake()
    {
        Assert.IsNotNull(orderList);

        if (logEvents)
        {
            OnSell.AddListener(pet => Debug.Log(nameof(OnSell) + ": " + pet.name));
        }
    }

    public bool TryPlace(Pet pet)
    {
        if (pet == null || !orderList.TryCompleteOrder(pet)) return false;

        OnSell.Invoke(pet);

        return true;
    }

    public Pet TryRemove() => null;
}