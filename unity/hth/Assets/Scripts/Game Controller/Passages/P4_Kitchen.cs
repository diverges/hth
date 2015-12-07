using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class P4_Kitchen : Passage {

    enum State
    {
        IDLE,
        IDLE_BAD,
        INIT,
        OVEN_ON,
        BAD_TONY,
        GOOD_TONY,
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

    public float DELAY_START = 2.0f;
    public float DELAY_OPTIONS = 2.0f;
    protected override IEnumerator UpdateState()
    {
        while (isActive)
        {
            switch (cState)
            {
                //  
                // Idle - Waiting for Player action
                case State.IDLE:
                    yield return null;
                    break;
                //
                // Initial State - Load Title
                case State.INIT:
                    yield return new WaitForSeconds(DELAY_START);
                    text[0].GetComponent<Typewriter>().LoadText(". . . ", 0.2f);
                    yield return StartCoroutine(Stall(text[0].GetComponent<Typewriter>()));

                    interactible[0].SetActive(true);
                    interactible[0].GetComponent<Typewriter>().LoadText(" Put handle back on.");
                    yield return StartCoroutine(Stall());
                    props[0].SetActive(false);
                    props[1].SetActive(true);   

                    text[0].GetComponent<Typewriter>().LoadText("Good as new...", 0.1f);
                    yield return StartCoroutine(Stall(text[0].GetComponent<Typewriter>()));

                    text[0].GetComponent<Typewriter>().LoadText("Oven is back in action!", 0.1f);
                    yield return StartCoroutine(Stall(text[0].GetComponent<Typewriter>()));

                    yield return new WaitForSeconds(DELAY_OPTIONS);

                    interactible[1].SetActive(true);
                    interactible[1].GetComponent<Typewriter>().SetText(" Alright, pizza time! ");

                    interactible[2].SetActive(true);
                    interactible[2].GetComponent<Typewriter>().SetText(" Pickup Tony's mess.");
 
                    cState = State.IDLE;
                    break;

                //
                //
                case State.BAD_TONY:
                    text[2].GetComponent<Typewriter>().LoadText("There's no way to clean this up before mom gets home.", 0.1f);
                    yield return StartCoroutine(Stall(text[2].GetComponent<Typewriter>()));

                    interactible[0].SetActive(true);
                    interactible[0].GetComponent<Typewriter>().LoadText(" Lock oven door");
                    yield return StartCoroutine(Stall());

                    text[0].GetComponent<Typewriter>().LoadText(". . .", 0.5f);
                    yield return StartCoroutine(Stall(text[0].GetComponent<Typewriter>()));

                    text[0].GetComponent<Typewriter>().LoadText("You've had enough pizza Tony!", 0.1f);
                    yield return StartCoroutine(Stall(text[0].GetComponent<Typewriter>()));

                    cState = State.IDLE;

                    break;

                case State.GOOD_TONY:
                    text[1].GetComponent<Typewriter>().LoadText("Tony needs the pizza more than me.", 0.08f);
                    yield return StartCoroutine(Stall(text[1].GetComponent<Typewriter>()));

                    interactible[0].SetActive(true);
                    interactible[0].GetComponent<Typewriter>().SetText(" Share Pizza");
                    yield return StartCoroutine(Stall());

                    text[0].GetComponent<Typewriter>().LoadText(". . .", 0.5f);
                    yield return StartCoroutine(Stall(text[0].GetComponent<Typewriter>()));

                    text[0].GetComponent<Typewriter>().LoadText("Rest in pizza, Tony.", 0.08f);
                    yield return StartCoroutine(Stall(text[0].GetComponent<Typewriter>()));

                    cState = State.IDLE;
                    break;
                // Default - Should not get here
                default:
                    yield return null;
                    break;
            }
        }
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

        // Load UI
        for (int i = 0; i < ui.Length; i++)
        {
            ui[i].SetActive(true);
            text[1].text = "";
        }

        foreach (GameObject e in interactible)
        {
            e.GetComponent<Typewriter>().enabled = true;
            e.GetComponent<Text>().text = "";
        }

        // Start State Coroutine
        StartCoroutine(UpdateState());
    }

    protected override void Act(GameActor objectid, Transform obj)
    {
        switch (objectid)
        {
            case GameActor.OBJ_K_OVEN:
                if (Input.GetButtonDown("Act") && cState == State.INIT)
                {
                    interactible[0].SetActive(false);
                    isWaiting = false;
                }
                else if (Input.GetButtonDown("Act") && (cState == State.BAD_TONY || cState == State.GOOD_TONY) )
                {
                    interactible[0].SetActive(false);
                    isWaiting = false;
                }
                break;
            case GameActor.P3_K_FRIDGE:
                if(Input.GetButtonDown("Act") && cState == State.IDLE) {
                    interactible[1].SetActive(false);
                    interactible[2].SetActive(false);
                    cState = State.GOOD_TONY;
                }
                break;
            case GameActor.P3_K_TRASH:
                if (Input.GetButtonDown("Act") && cState == State.IDLE)
                {
                    interactible[1].SetActive(false);
                    interactible[2].SetActive(false);
                    cState = State.BAD_TONY;
                }
                break;
            default:
                break;
        }
    }

    protected override void Clean(GameActor objectid, Transform obj)
    {
        // nothing to clean
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
