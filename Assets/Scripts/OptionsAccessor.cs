using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsAccessor : MonoBehaviour
{
    public void SetArcadeMode(bool arcadeMode) => OptionsHolder.Singleton.arcadeMode = arcadeMode;

    public void GoToTitle() => OptionsHolder.Singleton.GoToTitle();
    public void GoToGame() => OptionsHolder.Singleton.GoToGame();
}
