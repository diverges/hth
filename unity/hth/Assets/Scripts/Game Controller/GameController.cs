using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

    //
    // Passages
    public Passage[] passages;
    public ActiveObject cam;

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
        
	}

}
