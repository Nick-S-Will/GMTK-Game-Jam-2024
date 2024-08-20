
using System;
using UnityEngine.SceneManagement;

public class OptionsHolder : Holder<OptionsHolder>
{
    public string titleSceneName = "Title Scene", gameSceneName = "Game Scene";
    public bool arcadeMode = false;

    protected override void Awake()
    {
        base.Awake();

        if (Singleton == this) DontDestroyOnLoad(gameObject);
        else Singleton.arcadeMode = false;
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }

    public void GoToGame()
    {
        SceneManager.sceneLoaded += SetGameMode;
        SceneManager.LoadScene(gameSceneName);
    }

    private void SetGameMode(Scene scene, LoadSceneMode loadMode)
    {
        var scoreHandler = FindObjectOfType<ScoreHandler>();
        if (scoreHandler) scoreHandler.arcadeGameMode = arcadeMode;
        SceneManager.sceneLoaded -= SetGameMode;
    }
}
