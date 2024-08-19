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
    [Tooltip("Additional Time added")] [SerializeField] private float timeBonus = 60f;


    [Header("Events")]
    [Tooltip("Called offscreen; updates score")] public UnityEvent OnAdopted; 


    [Header("Debug")]
    [SerializeField] private bool logEvents;

    void Awake(){
        
        Assert.IsNotNull(progressHandler);


        if (logEvents){
            OnAdopted.AddListener(() => Debug.Log(nameof(OnAdoptedCalled)));
        }

    }
    void Update()
    {
        //if the score has exceeded the quota by the exceedingFactor and the score Hasn't exceeded quota yet
        if(((quota + exceedingFactor)==score) && !scoreExceededQuota) {
            AddTimeBonus();

            scoreExceededQuota = !scoreExceededQuota;
        }

        //if the score has already exceeded quota and the score has exceeded the last time by the factor
        else if(scoreExceededQuota && lastBonusScore + exceedingFactor == score){AddTimeBonus();}

    }

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


    private void AddTimeBonus(){
        progressHandler.AddTime(timeBonus);

        //for the Update()
        lastBonusScore = score;
    }




}
