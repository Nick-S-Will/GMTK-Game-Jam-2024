using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class PetBehaviour : MonoBehaviour, IDropPoint<Food>, IDragDroppable<Pet>
{
    [SerializeField] private SpriteRenderer petRenderer, foodRenderer;
    [SerializeField] private Sprite fullSprite;
    [Header("Feeding Settings")]
    [SerializeField][Min(0f)] private float feedingInterval = 10f;
    [Header("Stretch Settings")]
    [SerializeField][Min(1e-5f)] private float stretchFrequency = 1f;
    [SerializeField] Vector3 minStretch = Vector3.one, maxStretch = Vector3.one;
    [Header("Events")]
    public UnityEvent OnShowFood;
    public UnityEvent OnFeed, OnFull, OnGrab, OnDrop;
    [Header("Debug")]
    [SerializeField] private Pet pet;
    [SerializeField] private bool logEvents;

    private new Collider2D collider;
    private Vector3 startScale;
    private float timeSinceFeeding;
    private int foodIndex;

    public Pet Pet 
    {
        get => pet;
        set
        {
            pet = value;
            petRenderer.sprite = pet.sprite;
            petRenderer.color = pet.tint;
        }
    }
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
            OnGrab.AddListener(() => Debug.Log(nameof(OnGrab)));
            OnDrop.AddListener(() => Debug.Log(nameof(OnDrop)));
        }

        foodRenderer.gameObject.SetActive(false);
        collider = GetComponent<Collider2D>();
        startScale = petRenderer.transform.localScale;
    }

    private void Update()
    {
        timeSinceFeeding += Time.deltaTime;

        TryShowNextFood();
        UpdateStretch();
    }

    #region Mouse Messages
    private void OnMouseDown()
    {
        if (Pet == null) return;

        OnGrab.Invoke();

        collider.enabled = false;
    }

    private void OnMouseUp()
    {
        if (Pet == null) return;

        Drop(IDragDroppable<Ingredient>.MouseWorldPosition);

        collider.enabled = true;
    }

    private void OnMouseDrag()
    {
        if (Pet == null) return;

        Drag(IDragDroppable<Ingredient>.MouseWorldPosition);
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

    public Food TryRemove() => null;
    #endregion

    #region IDragDroppable
    Pet IDragDroppable<Pet>.ObjectReference
    {
        get => Pet;
        set => Pet = value;
    }

    public void Drag(Vector2 position)
    {
        transform.position = position;
    }

    public void Drop(Vector2 position)
    {
        if (IsShowingFood && IsFull && (this as IDragDroppable<Pet>).TryPlaceAtDropPoint(position) is PetSeller) Destroy(gameObject);
        else OnDrop.Invoke();
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
        foodRenderer.sprite = visible ? (IsFull ? fullSprite : Foods[foodIndex].bubbleSprite) : null;
        foodRenderer.gameObject.SetActive(visible);
    }
    #endregion

    #region Stretch
    private void UpdateStretch()
    {
        var interpolate = Mathf.PingPong(stretchFrequency * Time.time, 1f);
        var scale = Vector3.Lerp(minStretch, maxStretch, interpolate);
        petRenderer.transform.localScale = Vector3.Scale(scale, startScale);
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
