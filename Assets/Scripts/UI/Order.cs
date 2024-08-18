using Displayable;
using UnityEngine;
using UnityEngine.UI;

public class Order : MultiDisplay<Pet, PetIngredientDisplay, PetFoodDisplay>
{
    [SerializeField] private Slider remainingTimeDisplay;
    [SerializeField] private Image productDisplay;
    [Header("Base Values")]
    [SerializeField][Min(.1f)] private float baseCompletionTime = 30f;

    private float startTime;

    public float MaxCompletionTime { get => baseCompletionTime; set => baseCompletionTime = value; }
    public float RemainingTime => Mathf.Max(0f, startTime + MaxCompletionTime - Time.time);
    public float RemainingTimePercent => Mathf.Clamp01(RemainingTime / MaxCompletionTime);

    private void Awake()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        remainingTimeDisplay.value = RemainingTimePercent;
    }

    public override void UpdateGraphics()
    {
        productDisplay.sprite = displayObject.sprite;

        base.UpdateGraphics();
    }
}