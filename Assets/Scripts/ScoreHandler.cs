using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private ProgressHandler progressHandler; //ref to add time

    [Header("Victory Conditions")]
    [Tooltip("Minimum amount of adoptions to win")][Min(1f)] public int quota = 1;
    [Tooltip("Current amount of adoptions")][Min(0f)]public int score = 0;
    private bool scoreExceededQuota = false;
    private int lastBonusScore;

    [Tooltip("How much score must exceed for time bonus")][SerializeField] private int exceedingFactor = 5;
    [Tooltip("Additional Time added")] [SerializeField] private float timeBonus = 120f;

    //booleans Used for failure conditions
    public bool losingViaTime {get; private set;}

    [Header("Events")]

    public UnityEvent OnQuotaReached;
    [Tooltip("Called offscreen; updates score")] public UnityEvent OnAdopted; 

    [Header("Game Mode")]
    public bool arcadeGameMode = false; //exceeding quota increases time


    [Header("Debug")]
    [SerializeField] private bool logEvents;

    void Awake(){
        
        Assert.IsNotNull(progressHandler);

        if (logEvents){
            OnAdopted.AddListener(() => Debug.Log(nameof(OnAdoptedCalled)));
            OnQuotaReached.AddListener(() => Debug.Log(nameof(OnQuotaReachedCalled)));
        }

        //default values
        losingViaTime = false;
        lastBonusScore = quota;
    }
    void Update()
    {
        //if the quota has been reached for the first time
        if(quota == score && !scoreExceededQuota){OnQuotaReachedInvoked();}

        if(arcadeGameMode){
             //if the score has already exceeded quota and the score has exceeded the last time by the factor
            if(scoreExceededQuota && (lastBonusScore + exceedingFactor == score)){AddTimeBonus();}
        }
    }


    /* To add ScoreHandler system to game: 
        Connect OnAdoptedInvoked to OnSell(Pet) event in Pet Seller prefab (under Play Area GO) 
    */

    #region Adopted Event Handlers
    //anything can call on adopted event; first does score handler specific then inspector
    public void OnAdoptedInvoked(){
        OnAdoptedCalled();

        OnAdopted.Invoke();
    }

    private void OnAdoptedCalled(){
        score++;

        //add more functionality here or in inspector
    }
    #endregion


    //anything can call on quota reached event; but it deals with score handler tasks first before inspector
    #region Quota Event Handlers
    public void OnQuotaReachedInvoked(){
        OnQuotaReachedCalled();

        OnQuotaReached.Invoke();
    }

    private void OnQuotaReachedCalled(){
        //add more functionality here or in inspector

        //changes gamemode
        losingViaTime = true;
        scoreExceededQuota = true;
    }
    #endregion


    private void AddTimeBonus(){
        progressHandler.AddTime(timeBonus);

        //update as the adding-time-bonus-score goal has been reached
        lastBonusScore = score;
    }

}
