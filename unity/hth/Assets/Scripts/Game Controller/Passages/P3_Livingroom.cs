﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class P3_Livingroom : Passage {

    enum State
    {
        IDLE,
        INIT,
        BATHROOM,
        BATHROOM_MIRROR,
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

    //
    // Local Stopwatch
    private State cState;

    public float DELAY_START = 20.0f;
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
                    interactible[0].GetComponent<Text>().text = "Crumbs?";
                    interactible[1].GetComponent<Text>().text = "What the...";
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
                    text[3].GetComponent<Typewriter>().LoadText("PIZZA!!\nPIZZA!!\nPIZZA!");
                    yield return StartCoroutine(Stall(text[3].GetComponent<Typewriter>()));
                    interactible[5].SetActive(true);
                    interactible[5].GetComponent<Typewriter>().LoadText("There's pizza in\nthe kitchen...");
                    cState = State.IDLE;
                    break;
                case State.EXPLORE:
                    text[4].GetComponent<Typewriter>().LoadText("What happened here!?\nThe handle appears to be missing...");
                    yield return StartCoroutine(Stall(text[4].GetComponent<Typewriter>()));
                    interactible[8].SetActive(true); // fridge
                    interactible[9].SetActive(true); // trash
                    interactible[10].SetActive(true); // shelves
                    interactible[11].SetActive(true); // trash 2
                    interactible[13].SetActive(true); // bathtub
                    cState = State.IDLE;
                    break;
                case State.BATHTUB:
                    interactible[14].SetActive(false);
                    text[9].GetComponent<Typewriter>().LoadText(". . . ", 0.7f);
                    yield return StartCoroutine(Stall(text[9].GetComponent<Typewriter>()));
                    text[9].GetComponent<Typewriter>().LoadText("Wow, seriously? How did the handle get here.");
                    yield return StartCoroutine(Stall(text[9].GetComponent<Typewriter>()));
                    yield return new WaitForSeconds(1.0f);
                    interactible[13].SetActive(true);
                    interactible[13].GetComponent<Typewriter>().LoadText("Better take this back\nto the kitchen...");
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
        Typewriter tp = text[0].GetComponent<Typewriter>();
        while(isActive) {
            tp.LoadText("On this day in history, a competitive eater named Tony Baloney was reported dead");
            yield return StartCoroutine(Stall(tp));
            yield return new WaitForSeconds(1.0f);

            tp.LoadText("after winning a national pizza-eating contest. He broke a new personal record that day, at 84 slices.");
            yield return StartCoroutine(Stall(tp));
            yield return new WaitForSeconds(1.0f);

            tp.LoadText("\"Tony wasn't just an inspiration to competitive eaters worldwide,\" says Tony Baloney's former roommate.");
            yield return StartCoroutine(Stall(tp));
            yield return new WaitForSeconds(1.0f);

            tp.LoadText("\"He lived the dream.To him, everything was edible.Nothing was safe from his jaws.\"");
            yield return StartCoroutine(Stall(tp));
            yield return new WaitForSeconds(1.0f);

            tp.LoadText("He showed us the remnants of the room.");
            yield return StartCoroutine(Stall(tp));
            yield return new WaitForSeconds(1.0f);

            tp.LoadText("\"You see these bite marks on the walls, chairs, tables? Incredible.Strong jaws.\"");
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
            ui[i].SetActive(true);
        
        // Start State Coroutine
        StartCoroutine(UpdateState());
    }

    protected override void Act(GameActor objectid, Transform obj)
    {
        switch (objectid)
        {
            case GameActor.P3_CRUMBS:
                if (Input.GetButtonDown("Act"))
                {
                    interactible[0].SetActive(false);
                    text[1].GetComponent<Typewriter>().LoadText("These crumbs look fresh...\nWhere do they lead to?");
                    cState = State.BATHROOM;
                }
                break;
            case GameActor.P3_COUCH:
                if (Input.GetButtonDown("Act"))
                {
                    interactible[1].SetActive(false);
                    text[2].GetComponent<Typewriter>().LoadText("Looks like something tried to\ntake a bite out of this chair.\nIt's metal. Mustn't have\nbeen too bright...");
                }
                break;
            //
            // Handle Search
            // active objects when searching for handle
            case GameActor.P3_K_TRASH:
                if (Input.GetButtonDown("Act"))
                {
                    interactible[9].SetActive(false);
                    text[6].GetComponent<Typewriter>().LoadText("Nothing here...");
                    if (!interactible[8].activeSelf)
                    {
                        interactible[7].SetActive(true);
                        interactible[7].GetComponent<Typewriter>().LoadText("Maybe it's in the living room.");
                    }
                }
                break;
            case GameActor.P3_K_FRIDGE:
                if (Input.GetButtonDown("Act"))
                {
                    interactible[8].SetActive(false);
                    text[5].GetComponent<Typewriter>().LoadText("Nothing...\nMom needs to buy groceries.");
                    if (!interactible[9].activeSelf)
                    {
                        interactible[7].SetActive(true);
                        interactible[7].GetComponent<Typewriter>().LoadText("Maybe it's in the living room.");
                    }
                }
                break;
            case GameActor.P3_L_SHELVES:
                if (Input.GetButtonDown("Act"))
                {
                    interactible[10].SetActive(false);
                    text[7].GetComponent<Typewriter>().LoadText("Lots of books,\nbut no handle...");
                    if (!interactible[11].activeSelf)
                    {
                        interactible[12].SetActive(true);
                        interactible[12].GetComponent<Typewriter>().LoadText("How about the bathroom?");
                    }
                }
                break;
            case GameActor.P3_L_TRASH:
                if (Input.GetButtonDown("Act"))
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
                    interactible[14].GetComponent<Typewriter>().LoadText("This is crazy, but behind here maybe?");
                }
                else if (Input.GetButtonDown("Act")) {
                    interactible[14].SetActive(false);
                    cState = State.BATHTUB;    
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
                if (Input.GetButtonDown("Act"))
                {
                    interactible[5].SetActive(false);
                    Target = GameActor._GO_TO_LIVING_ROOM_BATH;
                    interactible[3].SetActive(true);
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
                    interactible[14].SetActive(true);
                    Target = GameActor._GO_TO_BATHROOM;
                }
                break;
            case GameActor.P3_E_KITCHEN:
                if (Input.GetButtonDown("Act"))
                {
                    Target = GameActor._GO_TO_KITCHEN;
                    isDone = true;
                }
                break;
            case GameActor._GO_TO_KITCHEN:
                if (Input.GetButtonDown("Act"))
                {
                    interactible[3].SetActive(false);
                    Target = GameActor._GO_TO_KITCHEN;
                    interactible[6].SetActive(true);
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
