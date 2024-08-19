using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class ProgressHandler : MonoBehaviour
{
    [SerializeField] private OrderList orderList;
    [Header("Wave Settings")]
    [SerializeField][Min(1f)] private int startOrderCount = 2;
    [SerializeField][Min(1f)] private float waveDuration = 60f, baseOrderInterval = 20f, baseOrderTimeLimit = 30f;
    [SerializeField][Range(0f, 1f)] private float speedUpScale = 0.95f;
    [SerializeField] private bool cancelRemainingOrdersOnWaveEnd;
    [Header("Events")]
    public UnityEvent OnWaveStart;
    public UnityEvent OnWaveEnd;
    [Header("Debug")]
    [SerializeField] private bool logEvents;

    private float elapsedWaveTime, elapsedOrderTime;
    private int clearedWaveCount;

    public float OrderInterval => Mathf.Pow(speedUpScale, clearedWaveCount) * baseOrderInterval;
    public float OrderTimeLimit => Mathf.Pow(speedUpScale, clearedWaveCount) * baseOrderTimeLimit;
    public float RemainingWaveTime => Mathf.Clamp(waveDuration - elapsedWaveTime, 0f, waveDuration);
    public int Wave => clearedWaveCount + 1;

    private void Awake()
    {
        Assert.IsNotNull(orderList);

        if (logEvents)
        {
            OnWaveStart.AddListener(() => Debug.Log(nameof(OnWaveStart)));
            OnWaveEnd.AddListener(() => Debug.Log(nameof(OnWaveEnd)));
        }

        for (int i = 0; i < startOrderCount; i++) orderList.GenerateOrder(OrderTimeLimit);
    }

    private void OnEnable()
    {
        orderList.ResumeOrders();
    }

    private void OnDisable()
    {
        if (orderList) orderList.PauseOrders();
    }

    private void Start()
    {
        OnWaveStart.Invoke();
    }

    private void Update()
    {
        CheckForWaveStart();

        elapsedWaveTime += Time.deltaTime;
        elapsedOrderTime += Time.deltaTime;

        CheckForNewOrder();

        CheckForWaveEnd();
    }

    private void CheckForWaveStart()
    {
        if (elapsedWaveTime != 0f) return;

        orderList.ResumeOrders();
        OnWaveStart.Invoke();
    }

    private void CheckForWaveEnd()
    {
        if (elapsedWaveTime < waveDuration) return;

        elapsedWaveTime = 0;
        elapsedOrderTime = 0f;
        clearedWaveCount++;

        if (cancelRemainingOrdersOnWaveEnd) orderList.CancelOrders();
        orderList.PauseOrders();
        OnWaveEnd.Invoke();

        Debug.Log(waveDuration); //reminder: need for game over screen later, also modify via score handler
    }

    private void CheckForNewOrder()
    {
        if (elapsedOrderTime < OrderInterval) return;

        orderList.GenerateOrder(OrderTimeLimit);
        elapsedOrderTime = 0f;
    }

    #region ScoreHandler Access Methods
    public void AddTime(float addBonus){
        Debug.Log("AddTime called");

        if(addBonus > 0){
            waveDuration += addBonus;
        }
    }

    #endregion

}