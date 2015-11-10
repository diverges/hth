using UnityEngine;
using System.Collections;

public enum GameActor {
    NONE, // denotes null actor
    DOOR,
    TEXT_1
}

public class ObjectID : MonoBehaviour {

    public GameActor uID;   // MUST be defined
                            // unique ID for interactable objects

}
