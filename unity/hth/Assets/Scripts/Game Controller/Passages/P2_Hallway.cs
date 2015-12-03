using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class P2_Hallway : Passage {

    enum State
    {
        IDLE, 
        INIT,
        LIGHT,
        WINDOW,
        ACTION_WAIT, // fork broom or picture path
        BROOM,
        LIVING_ROOM,
    };

    //
    // Scene Objects
    public GameObject[] props;

    //
    // Scene Text Objects
    public GameObject[] ui; // Text Canvas
    public Text[] text; // Text Elements

    //
    // Scene Interactible
    public GameObject[] interactible;

    //
    // Local Stopwatch
    private State cState;

    private Light lamp;

    //
    // Local Timing Parmeters
    public float DELAY_START = 4.0f;
    public float DELAY_LIGHT_SWITCH = 1.5f;
    public float DELAY_LIGHT_FLICKER = 3.0f;
    public float DELAY_LIGHT_FLICKER_2 = 2.5f;
    public float DELAY_WINDOW_TO_FOOTSTEP = 2.0f;
    public float DELAY_FOOTSTEP_TO_TEXT = 5.0f;

    protected override IEnumerator UpdateState()
    {
        while(isActive) {
            switch (cState)
            {
                //  
                // Idle - Waiting for Player action
                case State.IDLE:
                    yield return null;
                    break;
                case State.ACTION_WAIT:
                    yield return null;
                    break;
                //
                // Room Introduction
                case State.INIT:
                    props[1].GetComponent<AudioSource>().Play(); // Wind
                    props[2].GetComponent<AudioSource>().Stop(); // Stop footstep sound

                    interactible[0].SetActive(true);
                    yield return StartCoroutine(Stall());
                    interactible[0].SetActive(false);

                    ui[0].SetActive(true);
                    text[0].GetComponent<Typewriter>().LoadText(". . .", 0.5f);
                    yield return StartCoroutine(Stall(text[0].GetComponent<Typewriter>()));
                    text[0].GetComponent<Typewriter>().LoadText("It's dark in the house.\nAnd it feels colder than usual...");

                    yield return StartCoroutine(Stall(text[0].GetComponent<Typewriter>()));
                    interactible[1].SetActive(true);
                    yield return StartCoroutine(Stall());
                    interactible[1].SetActive(false);

                    ui[1].SetActive(true);
                    text[1].GetComponent<Typewriter>().LoadText("Hmm... is it always that\ndark down there?\nLet's get some\nlight in here.");

                    yield return StartCoroutine(Stall(text[1].GetComponent<Typewriter>()));
                    yield return new WaitForSeconds(DELAY_LIGHT_SWITCH);
                    interactible[1].SetActive(true);
                    interactible[1].GetComponent<Typewriter>().LoadText("turn lights on");
                    cState = State.IDLE;

                    break;
                //
                // Lights
                case State.LIGHT:
                    yield return new WaitForSeconds(DELAY_LIGHT_FLICKER);
                    ui[1].SetActive(true);

                    text[1].GetComponent<Typewriter>().LoadText("Umm... nothing?", 0.20f);
                    yield return StartCoroutine(Stall(text[1].GetComponent<Typewriter>()));
                    text[1].GetComponent<Typewriter>().LoadText("Why is the window open?");

                    interactible[3].SetActive(true);
                    ui[3].SetActive(true);

                    ui[0].SetActive(true);
                    text[0].text = "";
                    interactible[0].SetActive(true);
                    yield return StartCoroutine(Stall());
                    interactible[0].GetComponent<Typewriter>().LoadText("close window");
                    cState = State.IDLE;

                    break;
                //
                // Window
                case State.WINDOW:
                    ui[1].SetActive(false);
                    text[0].GetComponent<Typewriter>().LoadText("Weird, we don't ever open that window.");

                    yield return new WaitForSeconds(DELAY_WINDOW_TO_FOOTSTEP);
                    props[0].GetComponent<AudioSource>().Play();

                    yield return new WaitForSeconds(5.0f);
                    text[0].GetComponent<Typewriter>().LoadText("Is someone outside?", 0.1f);

                    yield return StartCoroutine(Stall(text[0].GetComponent<Typewriter>()));
                    // Enable possible actions
                    ui[2].SetActive(true);
                    ui[4].SetActive(true);

                    interactible[4].GetComponent<Text>().text = "Is someone there?";
                    interactible[4].SetActive(true);
                    cState = State.ACTION_WAIT;

                    break;
                //
                // Broom
                case State.BROOM:
                    yield return StartCoroutine(Stall(text[2].GetComponent<Typewriter>()));
                    interactible[4].SetActive(true);
                    cState = State.IDLE;
                    break;
                default:
                    break;
            }
        }
        yield return null;
    }

    IEnumerator FlickerLamp() {
        props[3].GetComponent<AudioSource>().PlayScheduled(2);

        lamp.enabled = true;
        float interval = 1.0f / 0.40f;
        for (float t = 0; t < 1.0f; t += Time.deltaTime * interval)
        {
            lamp.intensity = Mathf.Lerp(5.0f, 0.0f, t);
            lamp.enabled = true;
            yield return new WaitForSeconds(0.1f);
            lamp.enabled = false;
            yield return new WaitForSeconds(0.1f);
            yield return 0;
        }

        //props[3].GetComponent<AudioSource>().Stop();
    }

    public override void Cleanup()
    {
        isActive = false;

        // Hide all objects which are not currently needed
        foreach (GameObject e in interactible)
        {
            e.SetActive(false); // Hide interactible components
        }

        // Hide text canvas
        for (int i = 0; i < ui.Length; i++)
            ui[i].SetActive(false);


    }

    public override void Initialize(ActiveObject c)
    {
        Cleanup();
        isDone = false;
        target = GameActor.NONE;

        isActive = true;
        cam = c;
        cState = State.INIT;

        lamp = props[3].GetComponent<Light>();
        lamp.enabled = false;

        // Start State Coroutine
        StartCoroutine(UpdateState());
    }

    protected override void Act(GameActor objectid, Transform obj)
    {
        switch (objectid)
        {
            //
            // Interactables
            case GameActor.P2_LIGHTS:
                if (cState == State.INIT)
                {
                    isWaiting = false;
                }
                else if (Input.GetButtonDown("Act"))
                {
                    interactible[1].SetActive(false);
                    ui[0].SetActive(false);
                    ui[1].SetActive(false);
                    cState = State.LIGHT;
                    StartCoroutine(FlickerLamp());
                }
                break;
            case GameActor.P2_WINDOW:
                if(cState == State.INIT || cState == State.LIGHT) {
                    isWaiting = false;
                }
                else if (cState == State.IDLE && Input.GetButtonDown("Act"))
                {
                    props[1].GetComponent<AudioSource>().Stop();
                    interactible[0].SetActive(false);
                    cState = State.WINDOW;
                }
                break;
            case GameActor.P2_PICTURE:
                if (text[3].GetComponent<Text>().text.Length == 0)
                {
                    interactible[3].SetActive(false);
                    text[3].GetComponent<Typewriter>().LoadText("A happy looking family,\ncrowded around a dinner table.\nEveryone's smiling.");
                }
                break;
            case GameActor.P2_BROOM:
                if (Input.GetButtonDown("Act")) {
                    interactible[4].SetActive(false);
                    interactible[3].SetActive(false);
                    interactible[2].SetActive(false);
                    text[2].GetComponent<Typewriter>().LoadText("This isn't ideal\nbut it could help.");
                    cState = State.BROOM;
                }
                break;
            //
            // Room Transition
            case GameActor._GO_TO_LIVING_ROOM:
                if (Input.GetButtonDown("Act"))
                {
                    switch (cState) {
                        case State.ACTION_WAIT:
                            // TODO: Play faster footstep sound
                            text[4].GetComponent<Typewriter>().LoadText(". . . What was that!? ");
                            interactible[2].SetActive(true);
                            interactible[3].SetActive(false);
                            interactible[4].SetActive(false);
                            break;
                        default:
                            target = GameActor._GO_TO_LIVING_ROOM;
                            isDone = true;
                            break;
                    }           
                } 

                /*
                if (Input.GetButtonDown("Act"))
                {
                    target = GameActor._GO_TO_LIVING_ROOM;
                    isDone = true;
                }
                */
                break;
            default:
                break;
        }
    }

    protected override void Clean(GameActor objectid, Transform obj)
    {
        // Nothing to cleanup
        return;
    }

    // Use this for initialization
    void Start () {
        curID = GameActor.NONE;
        lastID = GameActor.NONE;
        activeObject = null;
    }
	
	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            // Update Room State
            //UpdateState();

            // Act Room Events
            curID = cam.GetActiveObjectID();
            Act(curID, cam.GetActiveObject());

            if (curID != lastID)
            {
                // Cleanup
                Clean(lastID, activeObject);

                // Update
                activeObject = cam.GetActiveObject();
                lastID = curID;
            }
        }
    }

}
