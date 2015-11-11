using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class CheckIfVR : MonoBehaviour {

    public GameObject vr_Cam;
    public GameObject p1_Cam;

	// Use this for initialization
	void Start () {
        if (VRSettings.loadedDevice != VRDeviceType.None)
        {
            Debug.Log("VR Device Detected");
            p1_Cam.SetActive(false);
        }
        else
        {
            Debug.Log("No VR Device Detected");
            vr_Cam.SetActive(false);
        }
	}

}
