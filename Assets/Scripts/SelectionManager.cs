using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectionManager : MonoBehaviour
{

    public GameObject myMenu;
    public GameObject myParent;
    MenuManager myMenuManager;
    public InputActionReference x_Button, a_Button;

    bool isColliding;
    Collider currentCollision;
    GameObject myDuplicate;

    // Start is called before the first frame update
    void Start()
    {
        myMenuManager = myMenu.GetComponent<MenuManager>();
        isColliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(myMenuManager.currMode == "SEL" && myParent.name.Contains("Left"))
        {
            if (x_Button.action.triggered && isColliding)
            {
                //Here we duplicate the object just to the side of itself
                //Debug.LogError("YOU'RE NOW INSIDE THE IF STATEMENT");
                GameObject governor = currentCollision.gameObject.transform.parent.gameObject;
                myDuplicate = Instantiate(governor);
                myDuplicate.transform.position += new Vector3(1, 0, 0);
            }
        }
        else
        {
            if (a_Button.action.triggered && isColliding)
            {
                //Here we duplicate the object just to the side of itself
                GameObject governor = currentCollision.gameObject.transform.parent.gameObject;
                myDuplicate = Instantiate(governor);
                myDuplicate.transform.position += new Vector3(1, 0, 0);
            }
        }
    }



    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("COLLISION WITH: " + collision.gameObject.name);
        if (collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.gameObject.name.Contains("Clone"))
        {
            //Debug.LogError("ITS WORKING LIKE YOU WANT");
            isColliding = true;
            currentCollision = collision;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        isColliding = false;
        currentCollision = null;
    }
}
