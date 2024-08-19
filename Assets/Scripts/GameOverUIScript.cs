using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class GameOverUIScript : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text quotaText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text totalTimeText;

    [Tooltip("Any side messages like 'new ingredients unlocked' or 'thanks for playing'")]
    [SerializeField] private TMP_Text endText;


    //takes the basic text for stats
    private string quotaPrefix, scorePrefix,totalTimePrefix;
    private void Awake()
    {
        Assert.IsNotNull(gameOverScreen);
        Assert.IsNotNull(titleText);
        Assert.IsNotNull(quotaText);
        Assert.IsNotNull(scoreText);
        Assert.IsNotNull(totalTimeText);
        Assert.IsNotNull(endText);

        quotaPrefix = quotaText.text;
        scorePrefix = scoreText.text;
        totalTimePrefix = totalTimeText.text;

        gameOverScreen.SetActive(false);
    }

    public void OnEnable(){
        //ref: waveText.text = wavePrefix + progressHandler.Wave;
        Debug.Log("Oh hey!");

    }

}
