using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ProgressHandlerUI : MonoBehaviour
{
    [SerializeField] private ProgressHandler progressHandler;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text timeText;

    private string wavePrefix, timePrefix;

    private void Awake()
    {
        Assert.IsNotNull(progressHandler);
        Assert.IsNotNull(waveText);
        Assert.IsNotNull(timeText);

        wavePrefix = waveText.text;
        timePrefix = timeText.text;
    }

    private void Update()
    {
        waveText.text = wavePrefix + progressHandler.Wave;
        timeText.text = timePrefix + Mathf.Ceil(progressHandler.RemainingWaveTime);
    }
}