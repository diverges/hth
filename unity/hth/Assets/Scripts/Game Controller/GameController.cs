using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

    //
    // Passages
    public Passage[] passages;
    public ActiveObject cam;
    public Transform camPos;

    int curPassage;
    Passage active = null;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        // Nothing here
        // Only one passage for now   
        if(active == null) {
            curPassage = 0;
            active = passages[0];
            active.Initialize(cam);
        }

        if(active.IsDone) {
            camPos.position = new Vector3(60, 12, 0);
            camPos.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        }
        
	}

}
