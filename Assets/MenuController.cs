using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    public ButtonController[] menuButtons;
    public Image selectionArrow;

    private int selectedIndex = 0;
    private Vector3 selectionArrowOffset = new Vector3(150, 0, 0);
    private MenuInputs inputs = new MenuInputs();

    private void Start()
    {
        selectionArrow.rectTransform.position = menuButtons[selectedIndex].GetComponent<Image>().rectTransform.position - selectionArrowOffset;
    }

    private void Update()
    {
        inputs.UpdateInputs();
        int verticalInput = inputs.Vertical();
        bool submitInput = inputs.Submit();

        if (verticalInput == 1) MoveUp();
        else if (verticalInput == -1) MoveDown();

        if (submitInput)
        {
            menuButtons[selectedIndex].ButtonFunction();
        }
    }

    private void MoveDown()
    {
        selectedIndex = (selectedIndex + 1) % menuButtons.Length;
        selectionArrow.rectTransform.position = menuButtons[selectedIndex].GetComponent<Image>().rectTransform.position - selectionArrowOffset;
    }

    private void MoveUp()
    {
        selectedIndex = (selectedIndex - 1) % menuButtons.Length;
        selectedIndex = (selectedIndex < 0) ? menuButtons.Length - 1 : selectedIndex;
        selectionArrow.rectTransform.position = menuButtons[selectedIndex].GetComponent<Image>().rectTransform.position - selectionArrowOffset;
    }

}
