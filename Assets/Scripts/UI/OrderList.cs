using UnityEngine;
using Displayable;
using System;

public class OrderList : DisplayMaker<Order, Pet>
{
    [SerializeField] private Pet[] pets;

    protected override Comparison<Order> DisplayComparison => new((order1, order2) => (int)(order1.RemainingTime - order2.RemainingTime));

    // TODO: Add order removal when out of time

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
}