using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineManager : MonoBehaviour {

    enum triggerStates {standby, enter, hold};
    private int prevTriggerState; 

    private MeshLineRenderer currLine;
    private int numClicks = 0;

	// Update is called once per frame
	void Update () {

        float rightTriggerVal = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);
        //Debug.Log(rightTriggerVal);

        // Right Trigger Enter
        if (rightTriggerVal > 0 && rightTriggerVal < 0.35f 
            && prevTriggerState == (int)triggerStates.standby)
        {
            prevTriggerState = (int)triggerStates.enter;

            GameObject go = new GameObject ();
            go.AddComponent<MeshRenderer>();
            currLine = go.AddComponent<MeshLineRenderer>();

            currLine.setWidth(.1f);

            numClicks = 0;
        }

        // Right Trigger Hold 
        else if (rightTriggerVal > 0.35f)
        {
            prevTriggerState = (int)triggerStates.hold;


            // global position of right controller
            Vector3 rightControllerPos = GameObject.Find("OVRCameraRig").transform.position
                + OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTrackedRemote);

            //currLine.SetVertexCount(numClicks + 1);
            //currLine.SetPosition(numClicks, rightControllerPos);

            currLine.AddPoint(rightControllerPos);

            numClicks++; 
        }

        // Right Trigger Standby
        else if (rightTriggerVal == 0)
        {
            prevTriggerState = (int)triggerStates.standby; 
        }
	}

}
