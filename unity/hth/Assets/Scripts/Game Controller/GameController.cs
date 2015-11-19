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
            switch(active.GetTarget) {
                case GameActor._GO_TO_BEDROOM:
                    camPos.position = new Vector3(2, 15, -37);
                    camPos.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    active = passages[0];
                    break;
                case GameActor._GO_TO_HALLWAY:
                    camPos.position = new Vector3(60, 12, 0);
                    camPos.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    active = passages[1];
                    active.Initialize(cam);
                    break;
                case GameActor._GO_TO_LIVING_ROOM:
                    camPos.position = new Vector3(110, 22, 185);
                    camPos.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    active = passages[2];
                    active.Initialize(cam);
                    break;
                case GameActor._GO_TO_KITCHEN:
                    camPos.position = new Vector3(42, 22, 328);
                    camPos.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                case GameActor._GO_TO_BATHROOM:
                    camPos.position = new Vector3(244, 22, 329);
                    camPos.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    break;
                default:
                    break;
            }   
        }
        
	}

}
