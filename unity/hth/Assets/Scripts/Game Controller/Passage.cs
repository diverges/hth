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

    protected bool isDone; // Has the player left the room
    protected GameActor target = GameActor.NONE; // target room

    protected ActiveObject cam; // current object being looked at
    protected bool isActive;
    protected Transform activeObject;
    protected GameActor curID, lastID;

    protected bool isWaiting = false;
    protected IEnumerator Stall()
    {
        isWaiting = true;
        while (isWaiting) yield return new WaitForSeconds(0.1f);
    }
    protected IEnumerator Stall(Typewriter p)
    {
        while (p.typing || p.fading) yield return new WaitForSeconds(0.1f);
    }

    /// <summary>
    /// Display all components and initialize them.
    /// </summary>
    public abstract void Initialize(ActiveObject c);
    public abstract void Cleanup();

    protected abstract IEnumerator UpdateState();
    protected abstract void Act(GameActor objectid, Transform obj);
    protected abstract void Clean(GameActor objectid, Transform obj);
    public bool IsDone {
        get { return isDone; }
    }
    public GameActor Target {
        get { return target; }
        set { target = value; }
    }

}
