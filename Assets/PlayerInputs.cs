using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs {

    // Final Inputs
    public int horizontal; //-1 left, 1 right
    public int vertical;   //-1 down, 1 up
    public bool jump;
    public bool dash;
    public bool sword;

    // Input down
    public bool newJumpDown = true;
    public bool newDashDown = false;

    // Last Inputs
    public int lastHorizontal;
    public int lastVertical;

    public enum InputMode {keyboard, ps4, xbox};
    public InputMode inputMode;

    public void UpdateInputs()
    {
        RetrieveInputs();
        RetrieveInputsDown();
        RememberLastInputs();
        
    }

    /*
    ** Update Inputs
    */

    private void RetrieveInputs()
    {
        horizontal = (int)Input.GetAxis("X");
        vertical = (int)Input.GetAxis("Y");
        jump = Input.GetButton("Jump");
        dash = Input.GetButton("Dash");
        sword = Input.GetButton("Attack");
    }

    private void RetrieveInputsDown()
    {
        if (Input.GetButtonDown("Jump")) newJumpDown = true;
        if (Input.GetButtonDown("Dash")) newDashDown = true;
    }

    private void RememberLastInputs()
    {
        if (horizontal != 0 || vertical != 0)
        {
            lastHorizontal = horizontal;
            lastVertical = vertical;
        }
    }

    /*
    ** Specific inputs functions
    */

    public bool IsNewJump()
    {
        return jump && newJumpDown;
    }

    public void UseNewJump()
    {
        newJumpDown = false;
    }

    public bool IsNewDash()
    {
        return dash && newDashDown;
    }
    
    public void UseNewDash()
    {
        newDashDown = false;
    }

}
