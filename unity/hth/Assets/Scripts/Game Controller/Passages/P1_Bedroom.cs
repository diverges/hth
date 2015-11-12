﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class P1_Bedroom : Passage {

    enum State {
        INIT,
        INIT_2,
        CALL,
        CALL_ANSWERED_1,
        CALL_ANSWERED_2,
        CALL_ANSWERED_3,
        CALL_ANSWERED_4,
        WAIT_5,
        FOOTSTEPS,
        END
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

    int ozy_verse = 0; // ozymandias poem state

    //
    // Local Stopwatch
    private Stopwatch timer = new Stopwatch();
    private State cState;

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
        
        // Hide text
    }

    public override void Initialize(ActiveObject c)
    {
        Cleanup();

        isActive = true;
        cam = c;
        cState = State.INIT;

        // Show t.v.
        ui[0].SetActive(true);
        text[0].text = "";
        text[1].text = "";

        timer.Start();
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
               if(timer.getElapsedSeconds() > 6.0f) {
                    text[0].GetComponent<Typewriter>().LoadText("Not\n Alone");
                    cState = State.INIT_2;
                }
                break;
            // 
            // Load Subtitle
            case State.INIT_2:
                if (timer.getElapsedSeconds() > 8.0f)
                {
                    text[1].GetComponent<Typewriter>().LoadText("A hypertext\nadventure game...");
                    cState = State.CALL;
                }
                break;
            //
            // Phone is ringing
            case State.CALL:
                if (timer.IsRunning && timer.getElapsedSeconds() > 7.0f) {
                    AudioSource phone = props[0].GetComponent<AudioSource>();
                    if (!phone.isPlaying) phone.Play();
                    if (timer.IsRunning) timer.Stop();
                    interactible[0].SetActive(true);
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
                            "Hope you’re feeling better.\n"
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
                            "your sister's recital.\n"
                        );
                }
                else if (timer.getElapsedSeconds() > 2.5f)
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
                            "spaghetti and meatballs." 
                        );
                }
                else if (timer.getElapsedSeconds() > 3.0f)
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
                            "Love you!" 
                        );
                }
                else if (timer.getElapsedSeconds() > 2.0f)
                {
                    interactible[1].SetActive(true);
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
        Text guiText;

        switch (objectid)
        {
            case GameActor.NONE:
                break;
            case GameActor.P1_ACT_PHONE:
                guiText = obj.GetComponent<Text>();
                guiText.color = Color.red;
                if (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetMouseButtonDown(0))
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
                guiText = obj.GetComponent<Text>();
                guiText.color = Color.red;
                if (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetMouseButtonDown(0))
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
                            cState = State.END;
                            break;
                    } 
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
            case GameActor.P1_ACT_A:
            case GameActor.P1_ACT_B:
            case GameActor.TEXT_1:
                Text guiText = obj.GetComponent<Text>();
                guiText.color = Color.blue;
                break;
            default:
                break;
        }
    }

}
