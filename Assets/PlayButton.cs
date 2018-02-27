using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : ButtonController
{
    public override void ButtonFunction()
    {
        SceneManager.LoadScene(1);
    }
}
