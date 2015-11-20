using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class P1_Bedroom : Passage {

    enum State {
        INIT,
        INIT_2,
        NEWS,
        CALL,
        CALL_ANSWERED_1,
        CALL_ANSWERED_2,
        CALL_ANSWERED_3,
        CALL_ANSWERED_4,
        FOOTSTEPS,
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
    private Stopwatch timer = new Stopwatch();
    private State cState;

    bool channel = false;
    int tvVerse = 0;

    // Coroutines
    // T.V.
    IEnumerator TV() {
        Typewriter tp = text[1].GetComponent<Typewriter>();
        while(props[0].activeSelf)
        {
            interactible[3].SetActive(true);
            if (!tp.typing)
            {
                if (channel)
                {
                    // Food
                    switch (tvVerse)
                    {
                        case 0:
                            tp.LoadText(" and once you have the\nonions chopped,\nadd them into\nthe sauce");
                            break;
                        case 1:
                            tp.LoadText(" and cook them until\nthey’re nice and\ncaramelized.");
                            break;
                    }
                    tvVerse++;
                    if (tvVerse >= 2) tvVerse = 0;
                }
                else
                {
                    // News
                    switch (tvVerse)
                    {
                        case 0:
                            tp.LoadText(" BREAKING\n NEWS We are getting reports\nof a sudden food shortage\nin our neighboring");
                            break;
                        case 1:
                            tp.LoadText(" town of Springfield. Sources indicate that\nall the food in grocery");
                            break;
                        case 2:
                            tp.LoadText(" stores and homes suddenly\ndisappeared overnight,\nleaving thousands\n");
                            break;
                        case 3:
                            tp.LoadText(" of families starving.\nFood shipments are on their way,");
                            break;
                        case 4:
                            tp.LoadText(" but be prepared\nfor a sudden influx\nof our neighbors\nin our grocery stores.");
                            break;
                    }
                    tvVerse++;
                    if (tvVerse >= 5) tvVerse = 0;
                }
            }
            yield return new WaitForSeconds(1.0f);
        }  
    }

    public override void Cleanup()
    {
        isActive = false;

        // Hide all objects which are not currently needed
        foreach(GameObject e in interactible) {
            e.SetActive(false); // Hide interactible components
        }

        // Hide text canvas
        for (int i = 0; i < ui.Length; i++)
            ui[i].SetActive(false);

        // Hide call id
        props[2].SetActive(false);

        // Stop Footstep Audio
        props[1].GetComponent<AudioSource>().Stop();
    }

    public override void Initialize(ActiveObject c)
    {
        Cleanup();
        isDone = false;
        target = GameActor.NONE;

        isActive = true;
        cam = c;
        cState = State.INIT;

        // Show t.v.
        ui[0].SetActive(true);
        text[0].text = "";
        text[1].text = "";

        text[1].GetComponent<Typewriter>().LoadText(" click\nto\nstart");
    }

    // Use this for initialization
    void Start () {
        curID = GameActor.NONE;
        lastID = GameActor.NONE;
        activeObject = null;
    }
	
	// Update is called once per frame
	void Update () {
        if(isActive) {
            // Update Room State
            UpdateState();

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

            if (timer.IsRunning) timer.Update();
        }
    }

    protected override void UpdateState()
    {
        switch (cState) {
            //
            // Initial State - Load Title
            case State.INIT:
                if(!timer.IsRunning && Input.GetButtonDown("Act")) {
                    timer.Start();
                    text[1].GetComponent<Typewriter>().Clear();
                    text[0].text = "";
                    text[1].text = "";
                }
                else if(timer.IsRunning && timer.getElapsedSeconds() > 1.0f) {
                    text[0].GetComponent<Typewriter>().LoadText("Not\n Alone");
                    cState = State.INIT_2;
                }
                break;
            // 
            // Load Subtitle
            case State.INIT_2:
                if ( timer.getElapsedSeconds() > 3.0f  && !text[1].GetComponent<Typewriter>().typing) {
                    text[1].GetComponent<Typewriter>().Fade(2.5f);
                    cState = State.NEWS;
                } 
                else if (timer.getElapsedSeconds() > 2.5f && !text[1].GetComponent<Typewriter>().typing)
                {
                    text[1].GetComponent<Typewriter>().LoadText("\n\n\n\n\n\n\nA hypertext\nadventure game...");
                }
                break;
            case State.NEWS:
                if (timer.getElapsedSeconds() > 10.0f)
                {
                    // Start T.V. Stream
                    interactible[3].SetActive(true);
                    text[1].GetComponent<Typewriter>().Clear();
                    text[0].text = "";
                    text[1].text = "";
                    StartCoroutine(TV());
                    cState = State.CALL;
                }
                break;  
            //
            // Phone is ringing
            case State.CALL:
                if (timer.IsRunning && timer.getElapsedSeconds() > 16f) {
                    AudioSource phone = props[0].GetComponent<AudioSource>();
                    if (!phone.isPlaying) phone.Play();
                    if (timer.IsRunning) timer.Stop();
                    interactible[0].SetActive(true);
                    props[2].SetActive(true);
                    // NS - Act on phone
                }
                break;
            //
            // You answered
            case State.CALL_ANSWERED_1:
                if(!timer.IsRunning) {
                    timer.Start();
                    text[2].GetComponent<Typewriter>().LoadText(
                            "Hi sweetie.\n" +
                            "Hope you’re feeling better.\n", props[4].GetComponent<AudioSource>()
                        );
                } else if (timer.getElapsedSeconds() > 2.5f) {
                    interactible[1].SetActive(true);
                }
                break;
            case State.CALL_ANSWERED_2:
                if (!timer.IsRunning)
                {
                    timer.Start();
                    text[2].GetComponent<Typewriter>().LoadText(
                            "We’re on our way to see\n" +
                            "your sister's recital.\n", props[4].GetComponent<AudioSource>()
                        );
                }
                else if (timer.getElapsedSeconds() > 3.0f)
                {
                    interactible[1].SetActive(true);
                }
                break;
            case State.CALL_ANSWERED_3:
                if (!timer.IsRunning)
                {
                    timer.Start();
                    text[2].GetComponent<Typewriter>().LoadText(
                            "I left dinner for you in\n" +
                            "the oven, your favorite:\n" +
                            "spaghetti and meatballs." , props[4].GetComponent<AudioSource>()
                        );
                }
                else if (timer.getElapsedSeconds() > 4.5f)
                {
                    interactible[1].SetActive(true);
                }
                break;
            case State.CALL_ANSWERED_4:
                if (!timer.IsRunning)
                {
                    timer.Start();
                    text[2].GetComponent<Typewriter>().LoadText(
                            "We’ll be back later tonight.\n" +
                            "Love you!" , props[4].GetComponent<AudioSource>()
                        );
                }
                else if (timer.getElapsedSeconds() > 2.5f)
                {
                    interactible[1].SetActive(true);
                }
                break;
            //
            // Wait 5 seconds
            case State.FOOTSTEPS:
                if(timer.getElapsedSeconds() > 5.0f) {
                    interactible[2].SetActive(true);                                     
                }
                break;
            // Default - Should not get here
            default:
                break;
        }
    }

    // When the object is active
    protected override void Act(GameActor objectid, Transform obj)
    {
        switch (objectid)
        {
            case GameActor.NONE:
                break;
            case GameActor.P1_ACT_PHONE:
                if (Input.GetButtonDown("Act"))
                {
                    // Go to Call State
                    interactible[0].SetActive(false);
                    AudioSource phone = props[0].GetComponent<AudioSource>();
                    phone.Stop();
                    ui[1].SetActive(true);
                    cState = State.CALL_ANSWERED_1;
                }
                break;
            case GameActor.P1_ACT_CONTINUE:
                if (Input.GetButtonDown("Act"))
                {
                    // Go to Call State
                    interactible[1].SetActive(false);
                    timer.Stop();
                    switch (cState) {
                        case State.CALL_ANSWERED_1: cState = State.CALL_ANSWERED_2; break;
                        case State.CALL_ANSWERED_2: cState = State.CALL_ANSWERED_3; break;
                        case State.CALL_ANSWERED_3: cState = State.CALL_ANSWERED_4; break;
                        case State.CALL_ANSWERED_4:
                            ui[1].SetActive(false);
                            props[2].SetActive(false);
                            timer.Start();
                            props[1].GetComponent<AudioSource>().Play();
                            cState = State.FOOTSTEPS;
                            break;
                    } 
                }
                break;
            case GameActor.P1_TV:
                if (text[1].text != "" && Input.GetButtonDown("Act")) {
                    channel = !channel;
                    tvVerse = 0;
                    text[1].GetComponent<Typewriter>().Clear();
                    interactible[3].SetActive(false);  
                }
                break;
            case GameActor.P1_DOOR:
                if (Input.GetButtonDown("Act")) {
                    target = GameActor._GO_TO_HALLWAY;
                    isDone = true;
                }  
                break;
            default:
                break;
        }
    }

    // When an object is no longer active
    protected override void Clean(GameActor objectid, Transform obj)
    {
        switch (objectid)
        {
            case GameActor.P1_ACT_PHONE:
            case GameActor.P1_ACT_CONTINUE:
            case GameActor.P1_TV:
            case GameActor.P1_DOOR:
            default:
                break;
        }
    }

}
