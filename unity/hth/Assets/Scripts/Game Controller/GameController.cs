using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

    private GameController[] interactibles;
    private Transform activeObject;
    private GameActor curID, lastID;


    public ActiveObject cam;

    //
    //
    // Local state Variables
    int ozy_verse = 0; // ozymandias poem state

	// Use this for initialization
	void Start () {
        curID = GameActor.NONE;
        lastID = GameActor.NONE;
        activeObject = null;
	}
	
	// Update is called once per frame
	void Update () {
        curID = cam.GetActiveObjectID();
        Act(curID, cam.GetActiveObject());

        if (curID != lastID) {
            // Cleanup
            Clean(lastID, activeObject);

            // Update
            activeObject = cam.GetActiveObject();
            lastID = curID;
        }
        
	}

    // When the object is active
    void Act(GameActor objectid, Transform obj) {
        switch (objectid) 
        {
            case GameActor.NONE:
                break;
            case GameActor.TEXT_1:
                Text guiText = obj.GetComponent<Text>();
                guiText.color = Color.red;
                Typewriter text = obj.GetComponent<Typewriter>();
                if (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetMouseButtonDown(0)) {
                    if (ozy_verse == 0 && !text.typing)
                    {
                        text.LoadText(
                            "Tell that its sculptor well those passions read\n" +
                            "Which yet survive, stamped on these lifeless things,\n" +
                            "The hand that mocked them, and the heart that fed:\n" +
                            "And on the pedestal these words appear:\n"
                        );
                        ozy_verse++;
                    }
                    else if (ozy_verse == 1 && !text.typing) {
                        text.LoadText(
                            "‘My name is Ozymandias, king of kings:\n" +
                            "Look on my works, ye Mighty, and despair!'\n"
                        );
                        ozy_verse++;
                    }
                    else if (!text.typing)
                        text.LoadText(
                            "Nothing beside remains.Round the decay\n" +
                            "Of that colossal wreck, boundless and bare\n" +
                            "The lone and level sands stretch far away."
                        );
                }
                break;
            case GameActor.DOOR:
                obj.GetComponent<Renderer>().material.color = Color.red;              
                break;
        }
    }


    // When an object is no longer active
    void Clean(GameActor objectid, Transform obj) {
        switch (objectid)
        {
            case GameActor.DOOR:
                obj.GetComponent<Renderer>().material.color = Color.blue;
                break;
            case GameActor.TEXT_1:
                Text guiText = obj.GetComponent<Text>();
                guiText.color = Color.blue;
                break;
            default:
                break;
        }
    }
}
