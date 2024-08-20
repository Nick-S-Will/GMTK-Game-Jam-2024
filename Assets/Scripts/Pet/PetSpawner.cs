using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class PetSpawner : MonoBehaviour, IDropPoint<Ingredient>
{
    [SerializeField] private PetBehaviour petPrefab;
    [SerializeField] private Transform petParent;
    [Header("Spawn Settings")]
    [SerializeField] private Bounds spawnBounds;
    [Header("Events")]
    public UnityEvent OnSpawn, OnClear;
    [Header("Debug")]
    [SerializeField] private bool logEvents;

    private List<PetBehaviour> pets = new();

    private void Awake()
    {
        Assert.IsNotNull(petPrefab);
        Assert.IsNotNull(petParent);

        if (logEvents)
        {
            OnSpawn.AddListener(() => Debug.Log(nameof(OnSpawn)));
            OnClear.AddListener(() => Debug.Log(nameof(OnClear)));
        }
    }

    #region IDropPoint
    public bool TryPlace(Ingredient obj)
    {
        if (obj == null || obj is not Pet pet) return false;

        Spawn(pet);

        return true;
    }

    public Ingredient TryRemove() => null;
    #endregion

    #region Spawing
    private void Spawn(Pet pet)
    {
        var position = spawnBounds.GetRandomPoint();
        var newPet = Instantiate(petPrefab, position, Quaternion.identity, petParent);
        newPet.Pet = pet;
        pets.Add(newPet);

        OnSpawn.Invoke();
    }

    [ContextMenu("Clear Pets")]
    public void ClearPets()
    {
        foreach (var pet in pets) Destroy(pet.gameObject);
        pets.Clear();

        OnClear.Invoke();
    }
    #endregion

    #region Debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(spawnBounds.center, spawnBounds.size);
    }
    #endregion
}