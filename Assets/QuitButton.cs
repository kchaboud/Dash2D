using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : ButtonController
{
    public override void ButtonFunction()
    {
        Application.Quit();
    }
}
