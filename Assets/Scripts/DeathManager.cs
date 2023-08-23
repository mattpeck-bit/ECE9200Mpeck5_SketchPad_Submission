using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{

    string[] nameSearch = { "Ellipse", "Circle", "Freehand", "Rectangle", "Square", "Straight"};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    { 
        //Debug.LogError("GETTING A COLLISION RUNNING THE DELETION CODE");
        GameObject governor = collision.gameObject.transform.parent.gameObject;
        //Debug.LogError("NAME IS: " + governor.name);
        if (governor.name.Contains("Clone"))
        {
            Destroy(governor);
            return;
        }
    }
}
