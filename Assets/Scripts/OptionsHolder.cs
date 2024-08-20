
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsHolder : Holder<OptionsHolder>
{
    public string titleSceneName = "Title Scene", gameSceneName = "Game Scene";
    public bool arcadeMode;

    protected override void Awake()
    {
        if (Singleton) Singleton.arcadeMode = false;

        base.Awake();

        if (Singleton == this) DontDestroyOnLoad(gameObject);
    }

    [ContextMenu("To Title")]
    public void GoToTitle()
    {
        if (!Application.isPlaying) return;

        SceneManager.LoadScene(titleSceneName);
    }

    [ContextMenu("To Game")]
    public void GoToGame()
    {
        if (!Application.isPlaying) return;

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
