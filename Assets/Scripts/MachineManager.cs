using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//has all functions of machine here, from processing to the drop and drag functionality
public class MachineManager : MonoBehaviour
{
    BaseMachineState currentState;

    #region Available States
    MachineBox machineBox = new MachineBox(); //already instatiated via monobehavior impl. in MachineBox 
    //todo: make slots state, processing abstract state -- which will depend on the enum type of the machine.
    #endregion

    // Start is called before the first frame update
    void Start()
    {
      currentState = machineBox; //causes the box to be awaiting the drop and drag mechanism
      currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        //todo: change current state based on what is occuring in game
    }
}
