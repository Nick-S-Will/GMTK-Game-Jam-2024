using UnityEngine;
using Displayable;
using System;
using System.Linq;
using UnityEngine.Events;

public class OrderList : DisplayMaker<Order, Pet>
{
    [SerializeField] private Pet[] pets;
    [Header("Events")]
    public UnityEvent OnNewOrder;
    public UnityEvent OnOrderExpire;

    protected override Comparison<Order> DisplayComparison => new((order1, order2) => (int)(order1.RemainingTime - order2.RemainingTime));

    public Pet[] OrdererdPets => displayInstances.Select(order => order.DisplayObject).ToArray();

    private void Update()
    {
        CancelExpiredOrders();
    }

    #region Add Orders
    [ContextMenu("Generate Order")]
    private void GenerateOrder()
    {
        if (Application.isPlaying) GenerateOrder(0f);
        else Debug.LogWarning($"Can't call {nameof(GenerateOrder)}() outside play mode");
    }

    public void GenerateOrder(float timeLimit = 0f)
    {
        var pet = pets[UnityEngine.Random.Range(0, pets.Length)];
        var order = MakeDisplay(pet);
        if (timeLimit > 0f) order.TimeLimit = timeLimit;

        OnNewOrder.Invoke();
    }
    #endregion

    #region Pause
    public void PauseOrders() => SetOrdersPaused(true);

    public void ResumeOrders() => SetOrdersPaused(false);

    public void SetOrdersPaused(bool paused)
    {
        displayInstances.RemoveAll(order => order == null);
        foreach (var order in displayInstances) order.enabled = !paused;
    }
    #endregion

    #region Cancel Orders
    private void CancelExpiredOrders() => CancelOrders(order => order.RemainingTime == 0f);

    public void CancelOrders() => CancelOrders(order => true);

    public void CancelOrders(Predicate<Order> predicate)
    {
        foreach (var order in Displays)
        {
            if (!predicate(order)) continue;

            DestroyDisplay(order);
            OnOrderExpire.Invoke();
        }
    }
    #endregion
}