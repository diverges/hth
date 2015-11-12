using UnityEngine;
using System.Collections;

public enum GameActor {
    NONE, // denotes null actor

    // Passage 1
    P1_ACT_PHONE,   
    P1_ACT_CONTINUE, 
    P1_ACT_A,
    P1_ACT_B,

    DOOR,
    TEXT_1
}

public class ObjectID : MonoBehaviour {

    public GameActor uID;   // MUST be defined
                            // unique ID for interactable objects

}
