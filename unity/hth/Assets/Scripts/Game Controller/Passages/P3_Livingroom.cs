using UnityEngine;
using System.Collections;
using System;

public class P3_Livingroom : Passage {

    public override void Cleanup()
    {
        isActive = false;
    }

    public override void Initialize(ActiveObject c)
    {
        Cleanup();
        isDone = false;
        target = GameActor.NONE;

        isActive = true;
        cam = c;
    }

    protected override void Act(GameActor objectid, Transform obj)
    {
        switch (objectid)
        {
            case GameActor._GO_TO_HALLWAY:
                if (Input.GetButtonDown("Act"))
                {
                    target = GameActor._GO_TO_LIVING_ROOM;
                    isDone = true;
                }
                break;
            case GameActor._GO_TO_KITCHEN:
                if (Input.GetButtonDown("Act"))
                {
                    target = GameActor._GO_TO_KITCHEN;
                    isDone = true;
                }
                break;
            case GameActor._GO_TO_BATHROOM:
                if (Input.GetButtonDown("Act"))
                {
                    target = GameActor._GO_TO_BATHROOM;
                    isDone = true;
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

    protected override void UpdateState()
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
        }
    }
}
