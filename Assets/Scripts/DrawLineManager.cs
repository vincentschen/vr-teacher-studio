using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineManager : MonoBehaviour {

    enum triggerStates {standby, enter, hold};
    private int prevTriggerState; 

    private LineRenderer currLine;

	// Update is called once per frame
	void Update () {

        float rightTriggerVal = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);
        //Debug.Log(rightTriggerVal);

        // right trigger
        if (rightTriggerVal > 0 && rightTriggerVal < 0.35f 
            && prevTriggerState == (int)triggerStates.standby)
        {
            Debug.Log("enter");
            prevTriggerState = (int)triggerStates.enter; 
        }
        else if (rightTriggerVal > 0.35f)
        {
            prevTriggerState = (int)triggerStates.hold; 
            Debug.Log("hold");
        }
        else if (rightTriggerVal == 0)
        {
            prevTriggerState = (int)triggerStates.standby; 
        }
	}

}
