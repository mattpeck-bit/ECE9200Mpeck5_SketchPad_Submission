using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    public GameObject theMenu, theButton;
    MenuManager myMenuManager;
    MeshRenderer myRenderer;
    public Material button, highlight;
    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myMenuManager = theMenu.GetComponent<MenuManager>();
        //Debug.Log("We printed the start function!");
    }

    // Update is called once per frame`
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "XRControllerLeft" || other.gameObject.name == "XRControllerRight")
        {
            //Debug.Log("Trigger Entered!");
            myMenuManager.ButtonPressed(theButton.GetComponentInChildren<TextMeshPro>().text);
            myRenderer.material = highlight;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        myRenderer.material = button;
    }
}
