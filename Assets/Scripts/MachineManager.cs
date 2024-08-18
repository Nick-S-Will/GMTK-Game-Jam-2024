using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//has all functions of machine here, from processing to the drop and drag functionality
public class MachineManager : MonoBehaviour, IDropHandler
{
#region Variables
    private Vector2 machinePosition;
    [Tooltip("Canvas that the ingredients are placed on")][SerializeField] private Canvas canvas;

#endregion

void Start(){
    machinePosition = GetComponent<RectTransform>().transform.position;
}

#region Slot 
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop called"+ machinePosition);

        if(eventData.pointerDrag != null){
            eventData.pointerDrag.GetComponent<RectTransform>().transform.position = (machinePosition);
            Debug.Log("Item recieved!" + eventData.pointerDrag.GetComponent<RectTransform>().transform.position);
        }
    }

#endregion
}
