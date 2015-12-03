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

    enum CameraPos {
        BEDROOM,
        HALLWAY,
        LIVING_ROOM,
        LIVING_ROOM_BATH,
        LIVING_ROOM_KITCHEN,
        BATHROOM,
        KITCHEN,
    };

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        // Nothing here
        // Only one passage for now   
        if(active == null) {
            //LoadPassage(0);

            // TEMP - SPAWN ON HALWAY
            LoadPassage(2, CameraPos.LIVING_ROOM);
        }

        if (active.IsDone) {
            switch(active.Target) {
                case GameActor._GO_TO_BEDROOM:
                    LoadCamera(CameraPos.BEDROOM);
                    break;
                case GameActor._GO_TO_HALLWAY:
                    LoadPassage(1, CameraPos.HALLWAY);
                    break;
                case GameActor._GO_TO_LIVING_ROOM:
                    LoadPassage(2, CameraPos.LIVING_ROOM);
                    break;
                case GameActor._GO_TO_KITCHEN:
                    LoadPassage(3, CameraPos.KITCHEN); //TODO
                    break;
                default:
                    break;
            }   
        } else if (!active.IsDone && active.Target != GameActor.NONE) {
            Debug.Log(active.Target);
            switch (active.Target)
            {
                case GameActor._GO_TO_BEDROOM:
                    LoadCamera(CameraPos.BEDROOM);
                    break;
                case GameActor._GO_TO_HALLWAY:
                    LoadCamera(CameraPos.HALLWAY);
                    break;
                case GameActor._GO_TO_LIVING_ROOM:
                    LoadCamera(CameraPos.LIVING_ROOM);
                    break;
                case GameActor._GO_TO_LIVING_ROOM_BATH:
                    LoadCamera(CameraPos.LIVING_ROOM_BATH);
                    break;
                case GameActor._GO_TO_LIVING_ROOM_KITCHEN:
                    LoadCamera(CameraPos.LIVING_ROOM_KITCHEN);
                    break;
                case GameActor._GO_TO_KITCHEN:
                    LoadCamera(CameraPos.KITCHEN);
                    break;
                case GameActor._GO_TO_BATHROOM:
                    LoadCamera(CameraPos.BATHROOM);
                    break;
                default:
                    break;
            }
            active.Target = GameActor.NONE;
        }
	}
    
    void LoadPassage(int passage) {
        if(active != null) active.Cleanup();
        active = passages[passage];
        active.Initialize(cam);
    }

    void LoadPassage(int passage, CameraPos pos)
    {
        if (active != null) active.Cleanup();
        active = passages[passage];
        active.Initialize(cam);
        LoadCamera(pos);
    }

    void LoadCamera(CameraPos pos) {
        switch(pos) {
            case CameraPos.BEDROOM:
                camPos.position = new Vector3(2, 15, -37);
                camPos.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
            case CameraPos.HALLWAY:
                camPos.position = new Vector3(140, 22, 34);
                camPos.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
            case CameraPos.LIVING_ROOM:
                camPos.position = new Vector3(110, 22, 440);
                camPos.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
            case CameraPos.LIVING_ROOM_BATH:
                camPos.position = new Vector3(195, 22, 490);
                camPos.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                break;
            case CameraPos.LIVING_ROOM_KITCHEN:
                camPos.position = new Vector3(100, 22, 529);
                camPos.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                break;
            case CameraPos.KITCHEN:
                camPos.position = new Vector3(42, 22, 568);
                camPos.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
            case CameraPos.BATHROOM:
                camPos.position = new Vector3(321, 22, 310);
                camPos.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                break;
        }
    }

}
