using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class P1_Bedroom : Passage {

    enum State {
        IDLE, // Default state when waiting for user action
        INIT,
        NEWS,
        CALL,
        CALL_ANSWERED,
        CALL_EXPIRED,
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

    public float DELAY_TITLE_TEXT = 1.0f;
    public float DELAY_SUBTITLE_TEXT = 2.5f;
    public float DELAY_SUBTITLE_TO_NEWS = 4.0f;
    public float DELAY_FADE_TO_NEWS = 4.0f;
    public float DELAY_NEWS_TO_CALL = 5.0f;
    public float DELAY_MOM_TALK = 1.5f;
    public float DELAY_CALL_TO_MUSIC = 2.0f;
    public float DELAY_CALL_TO_FOOTSTEPS = 5.0f;

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
                    isWaiting = true;
                    while (isWaiting)
                    {
                        if (Input.GetButtonDown("Act"))
                        {
                            isWaiting = false;
                            text[1].GetComponent<Typewriter>().Clear();
                            text[0].text = "";
                            text[1].text = "";
                        }
                        yield return new WaitForSeconds(0.01f);
                    }
 
                   
                    yield return new WaitForSeconds(DELAY_TITLE_TEXT);
                    text[0].GetComponent<Typewriter>().LoadText("Not\n Alone");

                    yield return new WaitForSeconds(DELAY_SUBTITLE_TEXT);
                    text[1].GetComponent<Typewriter>().LoadText("\n\n\n\n\n\n\nA hypertext\nadventure game...");

                    yield return new WaitForSeconds(DELAY_SUBTITLE_TO_NEWS);
                    text[1].GetComponent<Typewriter>().Fade(2.5f);
                    cState = State.NEWS;

                    break;
                case State.NEWS:
                    yield return new WaitForSeconds(DELAY_FADE_TO_NEWS);
                    // Start T.V. Stream
                    interactible[3].SetActive(true);
                    text[1].GetComponent<Typewriter>().Clear();
                    text[0].text = "";
                    text[1].text = "";
                    StartCoroutine(TV());
                    cState = State.CALL;
                    break;
                //
                // Phone is ringing
                case State.CALL:
                    yield return new WaitForSeconds(DELAY_FADE_TO_NEWS);
                    AudioSource phone = props[0].GetComponent<AudioSource>();
                    if (!phone.isPlaying) phone.Play();
                    if (timer.IsRunning) timer.Stop();
                    interactible[0].SetActive(true);
                    props[2].SetActive(true);
                    // NS - Act on phone
                    cState = State.IDLE;
                    break;
                //
                // You answered
                case State.CALL_ANSWERED:
                    text[2].GetComponent<Typewriter>().AppendText("Hi sweetie.\n", 
                        props[4].GetComponent<AudioSource>(), 0.05f);
                    text[2].GetComponent<Typewriter>().AppendText("Hope you’re feeling better.\n", 
                        props[4].GetComponent<AudioSource>(), 0.08f);

                    yield return StartCoroutine(Stall(text[2].GetComponent<Typewriter>()));
                    interactible[1].SetActive(true);
                    yield return StartCoroutine(Stall());


                    text[2].GetComponent<Typewriter>().LoadText(
                        "We’re on our way to see\n" +
                        "your sister's recital.\n", props[4].GetComponent<AudioSource>()
                    );

                    yield return StartCoroutine(Stall(text[2].GetComponent<Typewriter>()));
                    interactible[1].SetActive(true);
                    yield return StartCoroutine(Stall());
                    text[2].GetComponent<Typewriter>().LoadText(
                        "I left dinner for you in\n" +
                        "the oven, your favorite:"
                        , props[4].GetComponent<AudioSource>()
                    );
                    text[2].GetComponent<Typewriter>().AppendText(
                        " \n", props[4].GetComponent<AudioSource>(), 1.0f
                    );
                    text[2].GetComponent<Typewriter>().AppendText(
                        "pizza.", props[4].GetComponent<AudioSource>(), 0.10f
                    );

                    yield return StartCoroutine(Stall(text[2].GetComponent<Typewriter>()));
                    interactible[1].SetActive(true);
                    yield return StartCoroutine(Stall());
                    text[2].GetComponent<Typewriter>().LoadText(
                        "We’ll be back later tonight.\n" +
                        "Don't forget to do", props[4].GetComponent<AudioSource>()
                    );
                    yield return StartCoroutine(Stall(text[2].GetComponent<Typewriter>()));
                    yield return new WaitForSeconds(DELAY_MOM_TALK);

                    cState = State.CALL_EXPIRED;

                    break;
                case State.CALL_EXPIRED:
                    text[2].GetComponent<Typewriter>().LoadText(
                                            "your math homework tonight!\n" +
                                            "And take your hamper to the laundry room,", props[4].GetComponent<AudioSource>()
                                        );
                    yield return StartCoroutine(Stall(text[2].GetComponent<Typewriter>()));
                    yield return new WaitForSeconds(DELAY_MOM_TALK);
                    interactible[1].GetComponent<Text>().text = "Mom, no more...";
                    interactible[1].SetActive(true);

                    text[2].GetComponent<Typewriter>().LoadText(
                                            "I'm doing laundry tonight.\n" +
                                            "Separate your darks from your\n" + 
                                            "lights this time.", props[4].GetComponent<AudioSource>()
                                        );
                    yield return StartCoroutine(Stall(text[2].GetComponent<Typewriter>()));
                    yield return new WaitForSeconds(DELAY_MOM_TALK);

                    text[2].GetComponent<Typewriter>().LoadText(
                                            "We don't want to turn\n" +
                                            "another favorite shirt pink again,", props[4].GetComponent<AudioSource>()
                                        );
                    yield return StartCoroutine(Stall(text[2].GetComponent<Typewriter>()));
                    yield return new WaitForSeconds(DELAY_MOM_TALK);

                    text[2].GetComponent<Typewriter>().LoadText(
                                            "now do we? Love you!\n" +
                                            "Bye!", props[4].GetComponent<AudioSource>()
                                        );
                    yield return StartCoroutine(Stall(text[2].GetComponent<Typewriter>()));
                    yield return new WaitForSeconds(DELAY_MOM_TALK);


                    props[2].SetActive(false);                    
                    cState = State.FOOTSTEPS;

                    break;
                //
                // Wait 5 seconds
                case State.FOOTSTEPS:
                    ui[1].SetActive(false);
                    props[1].GetComponent<AudioSource>().Play();
                    yield return new WaitForSeconds(DELAY_CALL_TO_MUSIC);
                    props[5].GetComponent<AudioSource>().Play();
                    interactible[2].SetActive(true); 
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

        // Start State Coroutine
        StartCoroutine(UpdateState());
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
                    cState = State.CALL_ANSWERED;
                }
                break;
            case GameActor.P1_ACT_CONTINUE:
                if (Input.GetButtonDown("Act"))
                {
                    // Go to Call State
                    if (cState == State.CALL_ANSWERED) {
                        interactible[1].SetActive(false);
                        isWaiting = false;
                    } else if (cState == State.CALL_EXPIRED) {
                        cState = State.FOOTSTEPS;
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
        return;
    }

}
