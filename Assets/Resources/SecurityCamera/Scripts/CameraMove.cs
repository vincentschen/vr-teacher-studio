using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

    public Transform securityCamera;

    private Vector3 securityCameraStartPos;

    private GameObject vampire;

    void Start()
    {
        securityCameraStartPos = securityCamera.position;
        vampire = GameObject.Find("Vampire");
    }
	
	// Update is called once per frame
	void Update () 
    {
	    if(Input.GetKey(KeyCode.LeftArrow))
        {
            securityCamera.position = (Vector3)securityCamera.position + new Vector3(-0.1f, 0,0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            securityCamera.position = (Vector3)securityCamera.position + new Vector3(0.1f, 0,0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            securityCamera.position = (Vector3)securityCamera.position + new Vector3(0,0.1f,0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            securityCamera.position = (Vector3)securityCamera.position + new Vector3(0,-0.1f,0);
        }

	    if (Input.GetKeyDown(KeyCode.V))
	    {
	        ToggleVampire();
	    }


	   }

    void ToggleVampire()
    {
        if (vampire.layer == 8)
        {
            vampire.layer = 0;
        }
        else
        {
            vampire.layer = 8;
        }
    }
}
