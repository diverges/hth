using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class P3_Livingroom : Passage {

    enum State
    {
        IDLE,
        IDLE_COMPUTER,
        IDLE_BATHROOM,
        INIT,
        BATHROOM,
        BATHROOM_MIRROR,
        LR_COMPUTER,
        EXPLORE,
        BATHTUB,
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

    private Coroutine co;

    //
    // Local Stopwatch
    private State cState;

    public float DELAY_START = 5.0f;
    public float DELAY_TONY_EAT = 2.0f;
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
                    interactible[0].GetComponent<Text>().text = "";
                    interactible[1].GetComponent<Text>().text = "";
                    interactible[0].SetActive(true);
                    interactible[1].SetActive(true);
                    cState = State.IDLE;
                    break;
                case State.BATHROOM:
                    yield return StartCoroutine(Stall(text[1].GetComponent<Typewriter>()));
                    interactible[2].GetComponent<Text>().text = "Check the bathroom...";
                    interactible[2].SetActive(true);
                    cState = State.IDLE;
                    break;
                case State.BATHROOM_MIRROR:

                    //TODO: Play sound
                    props[4].GetComponent<AudioSource>().Play();
                    text[10].GetComponent<Typewriter>().LoadText(". . . ", 0.5f);
                    yield return new WaitForSeconds(DELAY_TONY_EAT);
                    interactible[13].SetActive(true);
                    interactible[13].GetComponent<Typewriter>().LoadText("W-W-What's going on?", 0.08f);
                    yield return StartCoroutine(Stall());

                    //TODO: Play sound
                    props[4].GetComponent<AudioSource>().Play();
                    text[10].GetComponent<Typewriter>().LoadText(" . . .", 0.5f);
                    yield return new WaitForSeconds(DELAY_TONY_EAT);
                    interactible[13].SetActive(true);
                    interactible[13].GetComponent<Typewriter>().LoadText("W-What do you want?", 0.1f);
                    yield return StartCoroutine(Stall());
                    text[10].GetComponent<Typewriter>().SetText("");

                    // change audio
                    AudioSource[] sounds = props[5].GetComponents<AudioSource>();
                    sounds[0].Stop();
                    sounds[1].Play();
                        

                    // draw mirror text
					props[4].GetComponent<AudioSource>().Play();
					text[3].GetComponent<Typewriter>().LoadText("      pizza", 0.1f);
                    yield return StartCoroutine(Stall(text[3].GetComponent<Typewriter>()));
                    text[3].GetComponent<Typewriter>().Fade(1.0f);
                    yield return StartCoroutine(Stall(text[3].GetComponent<Typewriter>()));

                    text[3].GetComponent<Typewriter>().LoadText("\n      pizza!!", 0.1f);
                    yield return StartCoroutine(Stall(text[3].GetComponent<Typewriter>()));
                    text[3].GetComponent<Typewriter>().Fade(1.0f);
                    yield return StartCoroutine(Stall(text[3].GetComponent<Typewriter>()));

                    text[3].GetComponent<Typewriter>().LoadText("\n\n      PIZZA!!!", 0.1f);
                    yield return StartCoroutine(Stall(text[3].GetComponent<Typewriter>()));
                    text[3].GetComponent<Typewriter>().Fade(1.0f);
                    yield return StartCoroutine(Stall(text[3].GetComponent<Typewriter>()));

                    text[3].GetComponent<Typewriter>().LoadText("\n\n\n   PIZZA!!!!!", 0.1f);
                    yield return StartCoroutine(Stall(text[3].GetComponent<Typewriter>()));
                    text[3].GetComponent<Typewriter>().Fade(1.0f);
                    yield return StartCoroutine(Stall(text[3].GetComponent<Typewriter>()));

					props[4].GetComponent<AudioSource>().Play();
					text[3].GetComponent<Typewriter>().LoadText("      TONY\n   BALONEY\n      WANT\n     PIZZA!!!", 0.05f);
                    yield return StartCoroutine(Stall(text[3].GetComponent<Typewriter>()));

                    interactible[5].SetActive(true);
                    interactible[5].GetComponent<Typewriter>().LoadText(". . . ", 0.5f);
                    yield return StartCoroutine(Stall(interactible[5].GetComponent<Typewriter>()));
                    interactible[5].GetComponent<Typewriter>().LoadText("What kind of name is that...?", 0.1f);
                    cState = State.IDLE;
                    break;
                case State.LR_COMPUTER:
                    interactible[15].GetComponent<Typewriter>().LoadText("Search 'Tony Baloney'");
                    yield return StartCoroutine(Stall());
                    interactible[15].GetComponent<Typewriter>().SetText("Read on...");
                    ui[2].SetActive(true);

                    text[11].GetComponent<Typewriter>().LoadText("August 15, 1955\n\n", 0.1f);
                    text[11].GetComponent<Typewriter>().AppendText("Springfield - Tragedy strikes Springfield's Annual Pizza Eating Contest", 0.05f);
                    yield return StartCoroutine(Stall(text[11].GetComponent<Typewriter>()));
                    interactible[15].SetActive(true);
                    yield return StartCoroutine(Stall());

                    text[11].GetComponent<Typewriter>().LoadText("this year, as competitive eater\nTony Baloney was pronounced\ndead shortly after winning\nthe contest.", 0.05f);
                    yield return StartCoroutine(Stall(text[11].GetComponent<Typewriter>()));
                    interactible[15].SetActive(true);
                    yield return StartCoroutine(Stall());

                    text[11].GetComponent<Typewriter>().LoadText("The autopsy reported the death\na result of \"asphyxia due to\nchoking and aspiration\nof gastric contents.\"", 0.05f);
                    yield return StartCoroutine(Stall(text[11].GetComponent<Typewriter>()));
                    interactible[15].SetActive(true);
                    yield return StartCoroutine(Stall());

                    text[11].GetComponent<Typewriter>().LoadText("Mr. Baloney had consumed\na record-breaking 84\nslices of pizza\nin the allotted 10 minutes.", 0.05f);
                    yield return StartCoroutine(Stall(text[11].GetComponent<Typewriter>()));
                    interactible[15].SetActive(true);
                    yield return StartCoroutine(Stall());

                    text[11].GetComponent<Typewriter>().LoadText("\"Tony wasn't self-destructive\nas much as he was...\ndim-witted.\" remarked\nTony's eating coach.", 0.05f);
                    yield return StartCoroutine(Stall(text[11].GetComponent<Typewriter>()));
                    interactible[15].SetActive(true);
                    yield return StartCoroutine(Stall());

					// TODO: Play sound effect
					interactible[3].GetComponent<Typewriter>().SetText("Tony...?");
					interactible[3].SetActive(true);
					// todo: start playing new music when player looks at "Tony...?"	

                    text[11].GetComponent<Typewriter>().LoadText("\"He embodied a truly sacrificial\napproach to eating,\nout of a deep - \nseated love for pizza.", 0.05f);
                    yield return StartCoroutine(Stall(text[11].GetComponent<Typewriter>()));
                    interactible[15].SetActive(true);
                    yield return StartCoroutine(Stall());

                    text[11].GetComponent<Typewriter>().LoadText("If it's any solace,\nI am certain this was\nthe way he would've\nwanted to go.\"", 0.05f);
                    yield return StartCoroutine(Stall(text[11].GetComponent<Typewriter>()));
                    interactible[15].GetComponent<Typewriter>().SetText("Close...");
                    interactible[15].SetActive(true);
                    yield return StartCoroutine(Stall());
                    text[11].GetComponent<Text>().text = "";
                    ui[2].SetActive(false);

                    cState = State.IDLE;
                    break;
                case State.EXPLORE:
                    interactible[15].SetActive(false);
                    isWaiting = false;

				text[4].GetComponent<Typewriter>().LoadText("The handle is missing");
                    yield return StartCoroutine(Stall(text[4].GetComponent<Typewriter>()));
                    interactible[8].SetActive(true); // fridge
                    interactible[9].SetActive(true); // trash
                    interactible[10].SetActive(true); // shelves
                    interactible[11].SetActive(true); // trash 2
                    //interactible[13].SetActive(true); // bathtub
                    cState = State.IDLE;
                    break;
                case State.BATHTUB:
                    interactible[13].SetActive(true);
                    interactible[13].GetComponent<Typewriter>().LoadText("Please not here");
                    yield return StartCoroutine(Stall());
                    props[8].SetActive(false);
                    props[9].SetActive(true);
                    props[9].GetComponent<AudioSource>().Play();
                    text[10].GetComponent<Typewriter>().LoadText(". . Good", 0.08f);

                    interactible[14].SetActive(true);
                    yield return StartCoroutine(Stall());
                    interactible[14].SetActive(false);
                    
                    props[2].SetActive(false);
                    props[3].SetActive(true);
                    props[3].GetComponent<AudioSource>().Play();
                    text[9].GetComponent<Typewriter>().LoadText(". . . ", 0.7f);
                    yield return StartCoroutine(Stall(text[9].GetComponent<Typewriter>()));
                    yield return new WaitForSeconds(1.0f);
                    interactible[16].SetActive(true);
                    interactible[16].GetComponent<Typewriter>().LoadText("Better take this back\nto the kitchen...");
                    cState = State.IDLE;
                    break;
                // Default - Should not get here
                default:
                    yield return null;
                    break;
            }
        }
    }

    private bool isTVOn = false;
    IEnumerator TV() {
        isTVOn = true;
        props[1].SetActive(true);
        Typewriter tp = text[0].GetComponent<Typewriter>();
        while(isActive) {
			tp.LoadText("Sitting alone starring at the wall in a room");
            yield return StartCoroutine(Stall(tp));
            yield return new WaitForSeconds(1.0f);

			tp.LoadText("The echoes in my head still ring of offensive gloom");
            yield return StartCoroutine(Stall(tp));
            yield return new WaitForSeconds(1.0f);

			tp.LoadText("\"Feeling unwelcome; a stranger in my own home");
            yield return StartCoroutine(Stall(tp));
            yield return new WaitForSeconds(1.0f);

			tp.LoadText("\"Still, it doesn't explain how I can morbidly lay to way past noon\"");
            yield return StartCoroutine(Stall(tp));
            yield return new WaitForSeconds(1.0f);

			tp.LoadText("Completely oblivious to the allure");
            yield return StartCoroutine(Stall(tp));
            yield return new WaitForSeconds(1.0f);

			tp.LoadText("\"A useless shadow in the night with only one way to go\"");
            yield return StartCoroutine(Stall(tp));
            yield return new WaitForSeconds(1.0f);

			tp.LoadText("\"Whatever the case I'm nowhere near the eye of the storm\"");
			yield return StartCoroutine(Stall(tp));
			yield return new WaitForSeconds(1.0f);
        }

        yield return null;
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

        // Hide text canvas
        for (int i = 0; i < text.Length; i++)
            text[i].text = "";

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
        for (int i = 0; i < ui.Length-1; i++)
            ui[i].SetActive(true);
        
        // Start State Coroutine
        co = StartCoroutine(UpdateState());
    }

    protected override void Act(GameActor objectid, Transform obj)
    {
        switch (objectid)
        {
            case GameActor.P3_CRUMBS:
                if (cState == State.IDLE && !text[1].GetComponent<Typewriter>().typing)
                {
                    interactible[0].SetActive(false);
                    text[1].GetComponent<Typewriter>().LoadText("These crumbs look fresh");
                    text[1].GetComponent<Typewriter>().AppendText(". . . \n", 0.5f);
                    text[1].GetComponent<Typewriter>().AppendText("Where do they lead?", 0.08f);
                    cState = State.BATHROOM;
                }
                break;
            case GameActor.P3_COUCH:
                if ((cState == State.IDLE || cState == State.BATHROOM) && !text[2].GetComponent<Typewriter>().typing)
                {
                    interactible[1].SetActive(false);
                    text[2].GetComponent<Typewriter>().LoadText("Bite marks, all over");
                }
                break;
            //
            // Handle Search
            // active objects when searching for handle
            case GameActor.P3_K_TRASH:
                if(interactible[9].GetComponent<Text>().text.Length == 0) {
                    interactible[9].GetComponent<Typewriter>().LoadText("Is it in the trash?");
                }
                else if (Input.GetButtonDown("Act"))
                {
                    interactible[9].SetActive(false);
                    text[6].GetComponent<Typewriter>().LoadText("Nothing here...");
                    if (!interactible[8].activeSelf)
                    {
                        interactible[7].SetActive(true);
                        interactible[7].GetComponent<Typewriter>().LoadText("Is it in the living room?");
                    }
                }
                break;
            case GameActor.P3_K_FRIDGE:
                if (interactible[8].GetComponent<Text>().text.Length == 0)
                {
                    interactible[8].GetComponent<Typewriter>().LoadText("Is it in the fridge?");
                }
                else if (Input.GetButtonDown("Act"))
                {
                    interactible[8].SetActive(false);
                    props[6].SetActive(false);
                    props[7].SetActive(true);
                    props[7].GetComponent<AudioSource>().Play();
                    text[5].GetComponent<Typewriter>().LoadText("Nothing...\nMom needs to buy groceries.");
                    if (!interactible[9].activeSelf)
                    {
                        interactible[7].SetActive(true);
                        interactible[7].GetComponent<Typewriter>().LoadText("Is it in the living room?");
                    }
                }
                break;
            case GameActor.P3_L_SHELVES:
                if (interactible[10].GetComponent<Text>().text.Length == 0)
                {
                    interactible[10].GetComponent<Typewriter>().LoadText("Can it be\nhere?");
                }
                else if (Input.GetButtonDown("Act"))
                {
                    interactible[10].SetActive(false);
                    text[7].GetComponent<Typewriter>().LoadText(" no books,\nno handle...");
                    if (!interactible[11].activeSelf)
                    {
                        interactible[12].SetActive(true);
                        interactible[12].GetComponent<Typewriter>().LoadText("How about the bathroom?");
                    }
                }
                break;
            case GameActor.P3_L_TRASH:
                if (interactible[11].GetComponent<Text>().text.Length == 0)
                {
                    interactible[11].GetComponent<Typewriter>().LoadText("In here?");
                }
                else if (Input.GetButtonDown("Act"))
                {
                    interactible[11].SetActive(false);
                    text[8].GetComponent<Typewriter>().LoadText("Nothing but trash here...");
                    if (!interactible[10].activeSelf)
                    {
                        interactible[12].SetActive(true);
                        interactible[12].GetComponent<Typewriter>().LoadText("How about the bathroom?");
                    }
                }
                break;
            case GameActor.P3_B_TUB:
                if (interactible[14].GetComponent<Text>().text == "") {
                    interactible[14].GetComponent<Typewriter>().LoadText("behind here?");
                }
                else if (Input.GetButtonDown("Act")) {
                    interactible[14].SetActive(false);
                    isWaiting = false;
                }
                break;
            case GameActor.P3_COMPUTER:
                if(cState == State.IDLE_COMPUTER) {
                    cState = State.LR_COMPUTER;
                } 
                else if (cState == State.LR_COMPUTER && Input.GetButtonDown("Act"))
                {
                    interactible[15].SetActive(false);
                    isWaiting = false;
                }
                break;
            //
            // Player Look at colliders
            // hidden objects that trigger text when the player
            // looks at them
            case GameActor.OBJ_BATH_MIRROR:
                interactible[4].SetActive(false);
                cState = State.BATHROOM_MIRROR;
                break;
            case GameActor.OBJ_K_OVEN:
                interactible[6].SetActive(false);
                cState = State.EXPLORE;
                break;
            case GameActor.OBJ_FS_TV:
                if (!isTVOn) { 
                    StartCoroutine(TV());
                }
                break;
            case GameActor._GO_TO_BATHROOM:
                if (Input.GetButtonDown("Act")) {
                    interactible[2].SetActive(false);
                    Target = GameActor._GO_TO_BATHROOM;
                    ui[0].SetActive(false);
                    ui[1].SetActive(false);
                    interactible[0].SetActive(false);
                    interactible[1].SetActive(false);
                    interactible[2].SetActive(false);
                    interactible[4].SetActive(true);
                } 
                break;
            case GameActor._GO_TO_LIVING_ROOM_BATH:
                if (cState == State.IDLE && Input.GetButtonDown("Act"))
                {
                    interactible[5].SetActive(false);
                    Target = GameActor._GO_TO_LIVING_ROOM_BATH;
                    interactible[15].SetActive(true);
                    cState = State.IDLE_COMPUTER;
                }
                break;
            case GameActor._GO_TO_LIVING_ROOM_KITCHEN:
                if (Input.GetButtonDown("Act"))
                {
                    Target = GameActor._GO_TO_LIVING_ROOM_KITCHEN;
                }
                break;
            case GameActor.P3_E_BATHROOM:
                if (Input.GetButtonDown("Act"))
                {
                    cState = State.BATHTUB;
                    Target = GameActor._GO_TO_BATHROOM;
                }
                break;
            case GameActor.P3_E_KITCHEN:
                if ((cState == State.BATHTUB || cState == State.BATHROOM_MIRROR) && Input.GetButtonDown("Act")) {
                    interactible[13].SetActive(false);
                    interactible[13].GetComponent<Text>().text = "";
                    isWaiting = false;
                }
                else if (cState == State.IDLE && Input.GetButtonDown("Act"))
                {
                    Target = GameActor._GO_TO_KITCHEN;
                    isDone = true;
                }
                break;
            case GameActor._GO_TO_KITCHEN:
                if (Input.GetButtonDown("Act"))
                {
                    interactible[3].SetActive(false);
                    text[11].GetComponent<Text>().text = "";
                    ui[2].SetActive(false);
                    interactible[6].SetActive(true);
                    text[11].GetComponent<Typewriter>().SetText("");
                    Target = GameActor._GO_TO_KITCHEN;
                    cState = State.IDLE;
                    StopCoroutine(co);
                    co = StartCoroutine(UpdateState()); // hack - restart co routine to stop pc text :)
                }
                break;
            default:
                break;
        }
    }

    protected override void Clean(GameActor objectid, Transform obj)
    {
        return;
    }

    // Use this for initialization
    void Start()
    {
        curID = GameActor.NONE;
        lastID = GameActor.NONE;
        activeObject = null;
    }

    // Update is called once per frame
    void Update()
    {
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
