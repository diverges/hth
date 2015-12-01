using UnityEngine;
using System.Collections;

public enum GameActor {
    NONE, // denotes null actor

    // Passage 1 - Room
    P1_ACT_PHONE,   
    P1_ACT_CONTINUE, 
    P1_TV,
    P1_DOOR,

    // Passage 2 - Hallway
    P2_LIGHTS,
    P2_WINDOW,
    P2_PICTURE,
    P2_BROOM,

    // Passage 3 - Living Room
    P3_CRUMBS,
    P3_COUCH,
    P3_K_TRASH,
    P3_K_FRIDGE,
    P3_L_TRASH,
    P3_L_SHELVES,
    P3_B_TUB,
    P3_E_BATHROOM,
    P3_E_KITCHEN,

    // Game Objects
    OBJ_FS_TV,
    OBJ_BATH_MIRROR,
    OBJ_K_OVEN,

    // Room Transition
    _GO_TO_BEDROOM,
    _GO_TO_HALLWAY,
    _GO_TO_LIVING_ROOM,
    _GO_TO_LIVING_ROOM_BATH,
    _GO_TO_LIVING_ROOM_KITCHEN,
    _GO_TO_KITCHEN,
    _GO_TO_BATHROOM,

}

public class ObjectID : MonoBehaviour {

    public GameActor uID;   // MUST be defined
                            // unique ID for interactable objects

}
