using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class SketchpadManager : MonoBehaviour
{
    //Prefabs to use for drawing
    public GameObject straightLineFab, freehandFab, circleFab, squareFab; 

    //In-scene Objects
    public GameObject myMenu;
    public GameObject myLeft;
    public GameObject myRight;
    public GameObject myCamera;
    public GameObject deathCapsule;

    //Placeholder to reference the current object being drawn
    GameObject placeholder;

    MenuManager myMenuManager;
    public InputActionReference a_Button = null, lGrip = null, rGrip = null, lTrigger = null, rTrigger = null, b_Button = null, x_Button = null, y_Button = null;
    bool priorFrame;
    Vector3 startingPos;
    private LineRenderer myStraightLineRenderer;
    private LineRenderer myFreeLineRenderer;
    private GameObject myCube;
    private GameObject mySphere;

    XRRayInteractor leftRayInteractor, rightRayInteractor;
    LineRenderer leftRenderer, rightRenderer;
    XRInteractorLineVisual leftInteractorLineVisual, rightInteractorLineVisual;

    RaycastHit iDontCare;

    [SerializeField]
    private float minDistance = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        myMenuManager = myMenu.GetComponent<MenuManager>();
        //myStraightLineRenderer = GetComponent<LineRenderer>();
        SpawnMenu();
        priorFrame = false;

        leftRayInteractor = myLeft.GetComponent<XRRayInteractor>();
        leftRenderer = myLeft.GetComponent<LineRenderer>();
        leftInteractorLineVisual = myLeft.GetComponent<XRInteractorLineVisual>();

        rightRayInteractor = myRight.GetComponent<XRRayInteractor>();
        rightRenderer = myRight.GetComponent<LineRenderer>();
        rightInteractorLineVisual = myRight.GetComponent<XRInteractorLineVisual>();
    }

    // Update is called once per frame
    void Update()
    {
        //Y button will be the menu button
        if (y_Button.action.triggered)
        {
            SpawnMenu();
        }
        if (myMenuManager.currMode == "SEL")
        {
            //If we're in selection mode, then we're not drawing, we instead want to enable our ability to manipulate objects
            if(myLeft.GetComponent<XRRayInteractor>().enabled == false || myRight.GetComponent<XRRayInteractor>().enabled == false)
            {
                leftRayInteractor.enabled = true;
                leftRenderer.enabled = true;
                leftInteractorLineVisual.enabled = true;
                rightRayInteractor.enabled = true;
                rightRenderer.enabled = true;
                rightInteractorLineVisual.enabled = true;
                deathCapsule.SetActive(true);
            }
        }
        else
        {
            if (myLeft.GetComponent<XRRayInteractor>().enabled == true || myRight.GetComponent<XRRayInteractor>().enabled == true)
            {
                leftRayInteractor.enabled = false;
                leftRenderer.enabled = false;
                leftInteractorLineVisual.enabled = false;
                rightRayInteractor.enabled = false;
                rightRenderer.enabled = false;
                rightInteractorLineVisual.enabled = false;
                deathCapsule.SetActive (false);
            }
            //When either trigger is pressed, it's time to draw
            if (lTrigger.action.IsPressed())
            {
                Disambiguate(myLeft);
            }
            if (rTrigger.action.IsPressed())
            {
                Disambiguate(myRight);
            }

            //When neither of the triggers are down anymore, we can reset the boolean
            if (!lTrigger.action.IsPressed() && !rTrigger.action.IsPressed()) { priorFrame = false; }
        }
    }

    public void SpawnMenu()
    {
        myMenu.SetActive(!myMenu.activeSelf);
        if (myMenu.activeSelf) { 
            myMenu.transform.position = myCamera.transform.position + (myCamera.transform.forward * 0.8f);
            myMenu.transform.LookAt(myCamera.transform, Vector3.up);
            myMenu.transform.Rotate(0, -90, 0);
        }
    }

    //This function looks at the menu status and tells us what we're drawing
    public void Disambiguate(GameObject theController)
    {
        switch (myMenuManager.currMode)
        {
            case "SL":
                DrawStraightLine(theController);
                break;
            case "FR":
                DrawFreeHand(theController);
                break;
            case "RS":
                DrawRectangleSquare(theController);
                break;
            case "EC":
                DrawEllipseCircle(theController);
                break;
            case "P":
                DrawPolygon(theController);
                break;
            default:
                break;
        }
    }

    public void DrawStraightLine(GameObject theController)
    {
        //Check to see if this is a first frame down
        if (!priorFrame)
        {
            //If it is, then we're going to instantiate a new line
            startingPos = theController.transform.position;
            placeholder = (GameObject)Instantiate(straightLineFab, new Vector3(0, 0, 0), Quaternion.identity);
            myStraightLineRenderer = placeholder.GetComponentInChildren<LineRenderer>();
            myStraightLineRenderer.SetPosition(0, theController.transform.position);
            priorFrame = true;
        }
        else //In the other case, we're actually drawing our line
        {
            placeholder.SetActive(true);
            myStraightLineRenderer.SetPosition(1, theController.transform.position);
            
        }
    }

    public void DrawFreeHand(GameObject theController)
    {
        //Check to see if this is a first frame down
        if (!priorFrame)
        {
            //If it is, then we're going to instantiate a new line
            startingPos = theController.transform.position;
            placeholder = (GameObject)Instantiate(freehandFab, new Vector3(0, 0, 0), Quaternion.identity);
            myFreeLineRenderer = placeholder.GetComponent<LineRenderer>();
            myFreeLineRenderer.SetPosition(0, startingPos);
            priorFrame = true;
        }
        else //In the other case, we're actually drawing our line
        {
            placeholder.SetActive(true);
            myFreeLineRenderer.positionCount++;
            myFreeLineRenderer.SetPosition(myFreeLineRenderer.positionCount-1, theController.transform.position);
        }
    }

    public void DrawRectangleSquare(GameObject theController)
    {
        //We're going to check to see if the rotation option is down, because then we'll rotate
        if ((theController.name == "RightHand Controller" && a_Button.action.IsPressed()) || (theController.name == "LeftHand Controller" && x_Button.action.IsPressed()))
        {
            placeholder.transform.rotation = theController.transform.rotation;
        }
        else //The other option is regular drawing
        {
            //Check to see if this is a first frame down
            if (!priorFrame)
            {
                //If it is, then we're going to instantiate a new object
                startingPos = theController.transform.position;
                placeholder = (GameObject)Instantiate(squareFab, new Vector3(0, 0, 0), Quaternion.identity);
                myCube = placeholder.transform.GetChild(0).gameObject;
                placeholder.transform.position = startingPos;

                priorFrame = true;
            }
            else //In the other case, we're actually drawing our object, so we need the size to match the location of the hand
            {
                //If the grip is down as well as the trigger, then we're drawing a square, not a rectangle
                if ((theController.name == "RightHand Controller" && rGrip.action.IsPressed()) || (theController.name == "LeftHand Controller" && lGrip.action.IsPressed()))
                {
                    placeholder.SetActive(true);
                    Vector3 tempDistance = (theController.transform.position - placeholder.transform.position);
                    float coeff = tempDistance.magnitude / (float)(Math.Sqrt(3.0));
                    placeholder.transform.localScale = new Vector3(coeff*Math.Sign(tempDistance.x), coeff*Math.Sign(tempDistance.y), coeff*Math.Sign(tempDistance.z));
                }
                else //But if just the trigger is down, then we're drawing a rectangle
                {
                    placeholder.SetActive(true);
                    placeholder.transform.localScale = theController.transform.position - placeholder.transform.position;
                }
            }
        }
    }

    public void DrawEllipseCircle(GameObject theController)
    {
        //We're going to check to see if the rotation option is down, because then we'll rotate
        if ((theController.name == "RightHand Controller" && a_Button.action.IsPressed()) || (theController.name == "LeftHand Controller" && x_Button.action.IsPressed()))
        {
            placeholder.transform.rotation = theController.transform.rotation;
        }
        else //The other option is regular drawing
        {
            //Check to see if this is a first frame down
            if (!priorFrame)
            {
                //If it is, then we're going to instantiate a new object
                startingPos = theController.transform.position;
                placeholder = (GameObject)Instantiate(circleFab, new Vector3(0, 0, 0), Quaternion.identity);
                mySphere = placeholder.transform.GetChild(0).gameObject;
                placeholder.transform.position = startingPos;

                priorFrame = true;
            }
            else //In the other case, we're actually drawing our object, so we need the size to match the location of the hand
            {
                //If the grip is down as well as the trigger, then we're drawing a sphere, not an ellipsoid
                if ((theController.name == "RightHand Controller" && rGrip.action.IsPressed()) || (theController.name == "LeftHand Controller" && lGrip.action.IsPressed()))
                {
                    placeholder.SetActive(true);
                    Vector3 tempDistance = (theController.transform.position - placeholder.transform.position);
                    float coeff = tempDistance.magnitude / (float)(Math.Sqrt(3.0));
                    placeholder.transform.localScale = new Vector3(coeff * Math.Sign(tempDistance.x), coeff * Math.Sign(tempDistance.y), coeff * Math.Sign(tempDistance.z));
                }
                else //But if just the trigger is down, then we're drawing an ellipsoid
                {
                    placeholder.SetActive(true);
                    placeholder.transform.localScale = theController.transform.position - placeholder.transform.position;
                }
            }
        }
    }

    public void DrawPolygon(GameObject theController)
    {
        //Check to see if this is a first frame down or if the b button has been pressed
        if (!priorFrame || b_Button.action.triggered)
        {
            //If it is, then we're going to instantiate a new line
            startingPos = theController.transform.position;
            placeholder = (GameObject)Instantiate(straightLineFab, new Vector3(0, 0, 0), Quaternion.identity);
            myStraightLineRenderer = placeholder.GetComponentInChildren<LineRenderer>();
            myStraightLineRenderer.SetPosition(0, theController.transform.position);
            priorFrame = true;
        }
        else //In the other case, we're actually drawing our line
        {
            placeholder.SetActive(true);
            myStraightLineRenderer.SetPosition(1, theController.transform.position);
        }
    }
}
