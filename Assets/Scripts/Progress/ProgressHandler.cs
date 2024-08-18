using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class ProgressHandler : MonoBehaviour
{
    [SerializeField] private OrderList orderList;
    [Header("Wave Settings")]
    [SerializeField][Min(1f)] private float waveDuration = 60f;
    [SerializeField][Min(1f)] private float baseOrderInterval = 20f;
    [SerializeField][Min(0f)] private int startOrderCount = 2;
    [SerializeField][Range(0f, 1f)] private float speedUpScale = 0.95f;
    [SerializeField] private bool cancelRemainingOrdersOnWaveEnd;
    [Header("Events")]
    public UnityEvent OnWaveStart;
    public UnityEvent OnWaveEnd;
    [Header("Debug")]
    [SerializeField] private bool logEvents;

    private float waveStartTime, waveEndTime, lastOrderTime;
    private int clearedWaveCount;

    public float OrderInterval => Mathf.Pow(speedUpScale, clearedWaveCount) * baseOrderInterval;
    public float RemainingWaveTime => Mathf.Clamp(waveEndTime - Time.time, 0f, waveDuration);
    public int Wave => clearedWaveCount + 1;

    private void Awake()
    {
        Assert.IsNotNull(orderList);

        if (logEvents)
        {
            OnWaveStart.AddListener(() => Debug.Log(nameof(OnWaveStart)));
            OnWaveEnd.AddListener(() => Debug.Log(nameof(OnWaveEnd)));
        }

        waveStartTime = Time.time;
        waveEndTime = waveStartTime + waveDuration;
        lastOrderTime = waveStartTime - OrderInterval;

        for (int i = 0; i < startOrderCount; i++) orderList.GenerateOrder(OrderInterval);
    }

    private void Start()
    {
        OnWaveStart.Invoke();
    }

    private void Update()
    {
        CheckForWaveStart();
        CheckForWaveEnd();

        CheckForNewOrder();
    }

    private void CheckForWaveStart()
    {
        if (waveStartTime < waveEndTime) return;

        waveEndTime = waveStartTime + waveDuration;

        OnWaveStart.Invoke();
    }

    private void CheckForWaveEnd()
    {
        if (Time.time < waveEndTime) return;

        waveStartTime = Time.time;
        clearedWaveCount++;

        if (cancelRemainingOrdersOnWaveEnd) orderList.CancelOrders();
        OnWaveEnd.Invoke();
    }

    private void CheckForNewOrder()
    {
        if (Time.time <= lastOrderTime + OrderInterval || RemainingWaveTime < OrderInterval) return;

        orderList.GenerateOrder(OrderInterval);
        lastOrderTime = Time.time;
    }
}