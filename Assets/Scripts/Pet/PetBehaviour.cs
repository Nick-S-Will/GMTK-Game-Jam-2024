using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class PetBehaviour : MonoBehaviour, IDropPoint<Food>
{
    [SerializeField] private Pet pet;
    [SerializeField] private SpriteRenderer petRenderer, foodRenderer;
    [SerializeField] private Sprite fullSprite;
    [Header("Feeding Settings")]
    [SerializeField][Min(0f)] private float feedingInterval = 10f;
    [Header("Events")]
    public UnityEvent OnShowFood;
    public UnityEvent OnFeed, OnFull, OnClick;
    [Header("Debug")]
    [SerializeField] private bool logEvents;

    private float timeSinceFeeding;
    private int foodIndex;

    public Pet Pet => pet;
    public Food[] Foods => pet.foods;
    public bool CanEat => foodIndex < Foods.Length && timeSinceFeeding >= feedingInterval;
    public bool IsShowingFood => foodRenderer.gameObject.activeSelf;
    public bool IsFull => foodIndex == Foods.Length;

    private void Awake()
    {
        Assert.IsNotNull(pet);
        Assert.IsNotNull(petRenderer);
        Assert.IsNotNull(foodRenderer);
        Assert.IsNotNull(fullSprite);

        if (logEvents)
        {
            OnShowFood.AddListener(() => Debug.Log(nameof(OnShowFood)));
            OnFeed.AddListener(() => Debug.Log(nameof(OnFeed)));
            OnFull.AddListener(() => Debug.Log(nameof(OnFull)));
            OnClick.AddListener(() => Debug.Log(nameof(OnClick)));
        }

        foodRenderer.gameObject.SetActive(false);
    }

    private void Update()
    {
        timeSinceFeeding += Time.deltaTime;

        TryShowNextFood();
    }

    #region Mouse Messages
    private void OnMouseDown()
    {
        _ = TryRemove();
    }
    #endregion

    #region Food Display
    private void TryShowNextFood()
    {
        if (timeSinceFeeding < feedingInterval || IsShowingFood) return;

        SetFoodVisibility(true);

        if (IsFull) OnFull.Invoke();
        else OnShowFood.Invoke();
    }

    private void HideFood() => SetFoodVisibility(false);
    
    private void SetFoodVisibility(bool visible)
    {
        foodRenderer.sprite = visible ? (IsFull ? fullSprite : Foods[foodIndex].sprite) : null;
        foodRenderer.gameObject.SetActive(visible);
    }
    #endregion

    #region IDropPoint
    public bool TryPlace(Food food)
    {
        if (!CanEat || food != Foods[foodIndex]) return false;

        HideFood();
        timeSinceFeeding = 0f;
        foodIndex++;

        OnFeed.Invoke();

        return true;
    }

    public Food TryRemove()
    {
        OnClick.Invoke();

        return null;
    }
    #endregion

    #region Debug
    private void OnValidate()
    {
        if (pet && petRenderer)
        {
            petRenderer.sprite = pet.sprite;
            petRenderer.color = pet.tint;
        }
    }
    #endregion
}
