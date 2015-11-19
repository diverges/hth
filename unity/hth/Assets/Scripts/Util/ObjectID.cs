using UnityEngine;
using System.Collections;

public enum GameActor {
    NONE, // denotes null actor

    // Passage 1
    P1_ACT_PHONE,   
    P1_ACT_CONTINUE, 
    P1_TV,
    P1_DOOR,


    // Room Transition
    _GO_TO_BEDROOM,
    _GO_TO_HALLWAY,
    _GO_TO_LIVING_ROOM,
    _GO_TO_KITCHEN,
    _GO_TO_BATHROOM,

}

public class ObjectID : MonoBehaviour {

    public GameActor uID;   // MUST be defined
                            // unique ID for interactable objects

}
