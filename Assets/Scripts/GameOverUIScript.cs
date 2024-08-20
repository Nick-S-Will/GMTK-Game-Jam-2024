using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class GameOverUIScript : MonoBehaviour
{

    [Header("Game Over Screen")]
    [SerializeField] private GameObject gameOverScreen;

    [Header("Handlers")]
    [SerializeField] private ProgressHandler progressHandler;
    [SerializeField] private ScoreHandler scoreHandler;


    [Header("Text Fields")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text quotaText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text totalTimeText;

    [Tooltip("Any side messages like 'new ingredients unlocked' or 'thanks for playing'")] [SerializeField] private TMP_Text endText;

    //takes the basic text for stats
    private string quotaPrefix, scorePrefix,totalTimePrefix;
    private void Awake()
    {
        Assert.IsNotNull(gameOverScreen);
        Assert.IsNotNull(progressHandler);
        Assert.IsNotNull(scoreHandler);

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
        quotaText.text = quotaPrefix + scoreHandler.quota;
        scoreText.text = scorePrefix + scoreHandler.score;
        totalTimeText.text = totalTimePrefix + progressHandler.StringWaveDuration;


        //custom end message conditions
        if(scoreHandler.losingViaTime){            
            endText.text = (scoreHandler.arcadeGameMode) ? "Thank you for playing!" : "New Ingredients Unlocked!";
        }
        else{
            endText.text = ("Shucks... Missed quota by "+ (scoreHandler.quota - scoreHandler.score));
        }
    
    }

}
