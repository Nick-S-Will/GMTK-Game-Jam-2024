using UnityEngine;
using Displayable;
using System;
using System.Linq;
using UnityEngine.Events;

public class OrderList : DisplayMaker<Order, Pet>
{
    [SerializeField] private Pet[] pets;
    [Header("Events")]
    public UnityEvent OnOrderExpire;

    protected override Comparison<Order> DisplayComparison => new((order1, order2) => (int)(order1.RemainingTime - order2.RemainingTime));

    public Pet[] OrdererdPets => displayInstances.Select(order => order.DisplayObject).ToArray();

    private void Update()
    {
        CancelExpiredOrders();
    }

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
        if (timeLimit > 0f) order.MaxCompletionTime = timeLimit;
    }

    private void CancelExpiredOrders()
    {
        foreach (var order in Displays)
        {
            if (order.RemainingTime > 0f) continue;

            DestroyDisplay(order);
            OnOrderExpire.Invoke();
        }
    }
}