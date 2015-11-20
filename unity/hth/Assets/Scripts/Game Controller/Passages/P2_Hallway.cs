using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class P2_Hallway : Passage {

    enum State
    {
        INIT,
        INIT_2,
        INIT_3,
        LIGHT,
        LIGHT_2,
        WINDOW,
        WINDOW_2,
        ACTION_WAIT, // fork broom or picture path
        BROOM,
        PICTURE,
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
    private Stopwatch timer = new Stopwatch();
    private State cState;

    
    private Light lamp;

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
    }

    protected override void Act(GameActor objectid, Transform obj)
    {
        switch (objectid)
        {
            //
            // Interactables
            case GameActor.P2_LIGHTS:
                if (Input.GetButtonDown("Act"))
                {
                    interactible[1].SetActive(false);
                    ui[0].SetActive(false);
                    ui[1].SetActive(false);
                    cState = State.LIGHT;
                    StartCoroutine(FlickerLamp());
                }
                break;
            case GameActor.P2_WINDOW:
                if (Input.GetButtonDown("Act"))
                {
                    props[1].GetComponent<AudioSource>().Stop();
                    props[2].GetComponent<AudioSource>().Play();
                    interactible[0].SetActive(false);
                    cState = State.WINDOW;
                }
                break;
            case GameActor.P2_PICTURE:
                if (Input.GetButtonDown("Act")) {
                    interactible[3].SetActive(false);
                    text[0].GetComponent<Typewriter>().LoadText("A happy looking family,\ncrowded around a dinner table.\nEveryone's smiling.");

                    interactible[4].GetComponent<Text>().text = "What's out there?";
                    interactible[4].SetActive(false);

                    cState = State.PICTURE;
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

    protected override void UpdateState()
    {
        switch(cState) {
            //
            // Room Introduction
            case State.INIT:
                if (!timer.IsRunning)
                {
                    timer.Start();
                    props[1].GetComponent<AudioSource>().Play();
                    props[2].GetComponent<AudioSource>().Stop();
                } 
                else if (timer.getElapsedSeconds() > 4.0f)
                {
                    timer.Stop();
                    ui[0].SetActive(true);
                    text[0].GetComponent<Typewriter>().LoadText("It's dark in the house.\nAnd it feels colder than usual...");
                    cState = State.INIT_2;
                }
                break;
            case State.INIT_2:
                if (!text[0].GetComponent<Typewriter>().typing) 
                {
                    ui[1].SetActive(true);
                    text[1].GetComponent<Typewriter>().LoadText("Hmm... is it always that\ndark down there?\nLet's get some\nlight in here.");
                    cState = State.INIT_3;
                }
                break;
            case State.INIT_3:
                if (!timer.IsRunning && !text[1].GetComponent<Typewriter>().typing)
                {
                    timer.Start();
                } else if(timer.IsRunning && timer.getElapsedSeconds() > 1.5f) {
                    timer.Stop();
                    interactible[1].SetActive(true);
                }
                break;
            //
            // Lights
            case State.LIGHT:
                // TODO: Light flicker
                if (!timer.IsRunning) timer.Start();
                else if (timer.getElapsedSeconds() > 3.0f)
                {
                    ui[1].SetActive(true);
                    text[1].GetComponent<Typewriter>().LoadText("Umm... nothing?\nWhy is the window open?");
                    cState = State.LIGHT_2;
                }
                break;
            case State.LIGHT_2:
                if(timer.IsRunning && timer.getElapsedSeconds() > 6.0f) {
                    timer.Stop();
                    ui[0].SetActive(true);
                    text[0].text = "";
                    interactible[0].SetActive(true);
                }
                break;
            //
            // Window
            case State.WINDOW:
                if (!timer.IsRunning) {
                    ui[1].SetActive(false);
                    timer.Start();
                    text[0].GetComponent<Typewriter>().LoadText("Weird, we don't ever open that window.");
                } else if(timer.getElapsedSeconds() > 2.0f) {
                    // TODO: Play footstep sound
                    props[0].GetComponent<AudioSource>().Play();
                    cState = State.WINDOW_2;
                }
                break;
            case State.WINDOW_2:
                if(timer.getElapsedSeconds() > 6.0f) {
                    text[0].GetComponent<Typewriter>().LoadText("Is someone outside?");

                   
                    cState = State.ACTION_WAIT;
                }
                break;
            case State.ACTION_WAIT:
                if(timer.IsRunning && !text[0].GetComponent<Typewriter>().typing) {
                    timer.Stop();

                    // Enable possible actions
                    ui[2].SetActive(true);
                    ui[3].SetActive(true);
                    ui[4].SetActive(true);

                    interactible[3].SetActive(true);
                    interactible[4].GetComponent<Text>().text = "Is someone there?";
                    interactible[4].SetActive(true);
                }
                break;
            //
            // Picture
            case State.PICTURE:
                if (!text[0].GetComponent<Typewriter>().typing) {
                    interactible[4].SetActive(true);
                }
                break;
            //
            // Broom
            case State.BROOM:
                if (!text[2].GetComponent<Typewriter>().typing)
                {
                    interactible[4].SetActive(true);
                }
                break;

            default:
                break;
        }
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

}
