using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

    //Based on: https://www.youtube.com/watch?v=BGr-7GZJNXg
public class DropDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //variables
    [Tooltip("Canvas that the ingredients are placed on")][SerializeField] private Canvas canvas;

    private RectTransform ingredientTransform;
    private CanvasGroup canvasGroup;


    private Vector2 startingPosition; 
    private Vector2 machinePosition; 


    // private Vector2 machinePosition = Vector2.zero; //temp until i get the machine going
    [Tooltip("The machine the ingredient is connected to")][SerializeField] private GameObject useableMachine; //temp until i get the machine going


    #region Methods
    void Awake(){
        ingredientTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        startingPosition = ingredientTransform.anchoredPosition;
    }

    void Start(){
        if(useableMachine == null){
            this.enabled = false;
            Debug.Log("No useable machine connected to ingredient; drag game object into inspector");
        }

        else{
            //get machine's position, if there is a connected machine to the ingredient
            machinePosition = (useableMachine.GetComponent<RectTransform>()).anchoredPosition;
        }

    }

    
     public void OnPointerDown(PointerEventData eventData){
       Debug.Log("PointerDown called");
    }

    public void OnBeginDrag(PointerEventData eventData){
       Debug.Log("OnBeginDrag called");

       canvasGroup.alpha = 0.5f;
       canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData){
       Debug.Log("OnEndDrag called");

        //basically, if it's not in the spot it should be... put it back in starting position
            //todo: Vector2.zero change to the machine spot later
            
        // if(ingredientTransform.anchoredPosition != machinePosition){
        //     Debug.Log(machinePosition);
        //     ingredientTransform.anchoredPosition = startingPosition;
        // }

        // else{
        //     ingredientTransform.anchoredPosition = machinePosition;
        // }

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }

    //from drag handler
    public void OnDrag(PointerEventData eventData){
        Debug.Log("Dragging");

        ingredientTransform.anchoredPosition += eventData.delta /canvas.scaleFactor;

    }
    #endregion


}
