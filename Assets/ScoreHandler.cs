using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class ScoreHandler : MonoBehaviour
{

    [Header("Victory Conditions")]
    [Tooltip("Minimum amount of adoptions to win")][Min(1f)] public int quota;
    [Tooltip("Current amount of adoptions")]public int score;
    [Tooltip("How much score must exceed for time bonus")][SerializeField] private int exceedingFactor = 5;
    [Tooltip("Additional Time added")] [SerializeField] private float timeBonus = 60f;

    [Header("Events")]
    [Tooltip("Called offscreen; updates score")] public UnityEvent OnAdopted; 


/*Plan:

When OnAdopted() is called, score +1




*/


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check quota, if exceeds score; if so by exceeding factor then timebonus! 
            //enable bool value for future checks, now checking based on the old score + exceeding factor for timebonus 
    }



}
