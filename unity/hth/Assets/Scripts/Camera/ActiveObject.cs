using UnityEngine;
using System.Collections;

public class ActiveObject : MonoBehaviour {

    private Transform activeObj;
    private ObjectID lookingAt;

	// Use this for initialization
	void Start () {
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        lookingAt = null;
        activeObj = null;
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1 << LayerMask.NameToLayer("CanClick"))) {
            lookingAt = (ObjectID) hit.transform.gameObject.GetComponent("ObjectID");
            activeObj = (lookingAt == null) ? null : hit.transform;
        } else {
            lookingAt = null;
            activeObj = null;
        }

	}

    public GameActor GetActiveObjectID() {
        if (lookingAt == null)
        {
            return GameActor.NONE;
        }
        else
        {
            return lookingAt.uID;
        }
    }

    public Transform GetActiveObject()
    {
        return activeObj;
    }
}
