using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //variables
    [SerializeField] private Canvas canvas;
    private RectTransform ingredientTransform;


    #region Methods
    void Awake(){
        ingredientTransform = GetComponent<RectTransform>();
    }

    //Based on: https://www.youtube.com/watch?v=BGr-7GZJNXg
    
     public void OnPointerDown(PointerEventData eventData){
       Debug.Log("PointerDown called");
    }

    public void OnBeginDrag(PointerEventData eventData){
       Debug.Log("OnBeginDrag called");
    }

    public void OnEndDrag(PointerEventData eventData){
       Debug.Log("OnEndDrag called");
    }

    //from drag handler
    public void OnDrag(PointerEventData eventData){
        Debug.Log("Dragging");
        
        ingredientTransform.anchoredPosition += eventData.delta /canvas.scaleFactor;

    }
    #endregion


}
