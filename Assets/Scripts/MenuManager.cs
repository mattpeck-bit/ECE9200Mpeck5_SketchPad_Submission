using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    string[] modes = { "SL", "FR", "RS", "EC", "P", "SEL" };
    public GameObject displayTextObject;
    TextMeshPro displayText;
    public string currMode = null;
    // Start is called before the first frame update
    void Start()
    {
        displayText = displayTextObject.GetComponent<TextMeshPro>();
        currMode = modes[0];
        displayText.text = "StraightLine";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPressed(string buttonText)
    {
        switch (buttonText){
            case "Straight Line":
                currMode = modes[0];
                dummyFunction(buttonText);
                break;
            case "Freehand":
                currMode = modes[1];
                dummyFunction(buttonText);
                break;
            case "Rectangle/Square":
                currMode=modes[2];
                dummyFunction(buttonText);
                break;
            case "Ellipse/Circle":
                currMode = modes[3];
                dummyFunction(buttonText);
                break;
            case "Polygons":
                currMode = modes[4];
                dummyFunction(buttonText);
                break;
            case "Select":
                currMode = modes[5];
                dummyFunction(buttonText);
                break;
            default:
                Debug.LogError("MENU ERROR: BUTTON TEXT NOT CORRESPONDING PROPERLY");
                break;
        }
    }

    void dummyFunction(string buttonText)
    {
        displayText.text = buttonText;
    }
}
