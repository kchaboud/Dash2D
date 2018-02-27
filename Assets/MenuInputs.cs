using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputs {

    // Final Inputs
    private int horizontal; //-1 left, 1 right
    private int vertical;   //-1 down, 1 up
    private int lastHorizontal;
    private int lastVertical;
    private bool submit;

    public void UpdateInputs()
    {
        lastHorizontal = horizontal;
        lastVertical = vertical;
        horizontal = (int)Input.GetAxis("X");
        vertical = (int)Input.GetAxis("Y");
        submit = Input.GetButtonDown("Jump");;
    }

    public int Vertical()
    {
        if (lastVertical == 0)
        {
            return vertical;
        }
        return 0;
    }

    public bool Submit()
    {
        return submit;
    }
}
