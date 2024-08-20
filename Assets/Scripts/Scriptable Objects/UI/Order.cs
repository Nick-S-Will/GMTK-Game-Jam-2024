using Displayable;
using UnityEngine;
using UnityEngine.UI;

public class Order : MultiDisplay<Pet, PetIngredientDisplay, PetFoodDisplay>
{
    [SerializeField] private Slider remainingTimeDisplay;
    [SerializeField] private Image productDisplay;
    [Header("Base Values")]
    [SerializeField][Min(.1f)] private float baseTimeLimit = 30f;
    [Header("Debug")]
    [SerializeField] private Pet pet;
    [SerializeField] private bool displayOnly;

    private float elapsedTime;

    public float TimeLimit { get => baseTimeLimit; set => baseTimeLimit = value; }
    public float RemainingTime => Mathf.Max(0f, TimeLimit - elapsedTime);
    public float RemainingTimePercent => Mathf.Clamp01(RemainingTime / TimeLimit);

    private void Start()
    {
        if (pet) SetObject(pet);
        if (displayOnly) enabled = false;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        remainingTimeDisplay.value = RemainingTimePercent;
    }

    public override void UpdateGraphics()
    {
        productDisplay.sprite = displayObject.sprite;
        productDisplay.color = displayObject.tint;

        base.UpdateGraphics();
    }
}