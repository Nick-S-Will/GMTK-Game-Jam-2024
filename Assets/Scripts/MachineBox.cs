using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//manages the drop and drag interactions

//add state later
public class MachineBox : BaseMachineState, IDropHandler
{
    #region Methods from BaseMachineState
    public override void EnterState(MachineManager machineManager){
        Debug.Log("Entered the Machine Box state");

    }
    public override void ExitState(MachineManager machineManager){
         Debug.Log("Exited the Machine Box state");

    }
    #endregion

#region MachineBox Specific 
    public void OnDrop(PointerEventData data)
    {
        Debug.Log("OnDrop called");
    }
    #endregion

}
