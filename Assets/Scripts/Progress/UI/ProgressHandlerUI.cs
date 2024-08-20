using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ProgressHandlerUI : MonoBehaviour
{

    [Header("Handlers")]
    [SerializeField] private ProgressHandler progressHandler;
    [SerializeField] private ScoreHandler scoreHandler;


    [Header("Text Fields")]
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text scoreText;


    private string wavePrefix, timePrefix, scorePrefix;

    private void Awake()
    {
        Assert.IsNotNull(progressHandler);
        Assert.IsNotNull(waveText);
        Assert.IsNotNull(timeText);
        Assert.IsNotNull(scoreText);


        wavePrefix = waveText.text;
        timePrefix = timeText.text;
        scorePrefix = scoreText.text;

    }

    private void Update()
    {
        waveText.text = wavePrefix + progressHandler.Wave;
        timeText.text = timePrefix + Mathf.Ceil(progressHandler.RemainingWaveTime);
        scoreText.text = scorePrefix + scoreHandler.score;
    }
}