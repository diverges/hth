using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Passages handle current game scene and interactable elements.
/// </summary>
public abstract class Passage : MonoBehaviour {


	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

    protected bool isDone;
    protected ActiveObject cam; // current object being looked at
    protected bool isActive;
    protected Transform activeObject;
    protected GameActor curID, lastID;

    /// <summary>
    /// Display all components and initialize them.
    /// </summary>
    public abstract void Initialize(ActiveObject c);
    public abstract void Cleanup();

    protected abstract void UpdateState();
    protected abstract void Act(GameActor objectid, Transform obj);
    protected abstract void Clean(GameActor objectid, Transform obj);
    public bool IsDone {
        get { return isDone; }
    }

}
