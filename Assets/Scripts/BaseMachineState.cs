using System.Collections;
using System.Collections.Generic;

public abstract class BaseMachineState
{
    public abstract void EnterState(MachineManager machineManager);
    public abstract void ExitState(MachineManager machineManager);

}
