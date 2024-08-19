using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Collider2D))]
public class MachineButton : MonoBehaviour
{
    [SerializeField] private MachineManager machine;

    private void Awake()
    {
        Assert.IsNotNull(machine);
    }

    private void OnMouseDown()
    {
        machine.TryProcessIngredients();
    }
}