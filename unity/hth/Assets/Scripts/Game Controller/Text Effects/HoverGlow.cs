using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HoverGlow : MonoBehaviour {

    public Text text;
    public bool isOver = false;
    private int size;


    Stopwatch timer = new Stopwatch();

	// Use this for initialization
	void Start () {
        isOver = false;
        size = text.fontSize;
	}

    // Update is called once per frame
    float t = 0.0f;
    Color myColor = new Color();
    Color cColor = new Color();
    bool isChanging = false;
	void Update () {
        if (!timer.IsRunning) timer.Start();
        else timer.Update();

	    if(isOver) {
            if (timer.getElapsedSeconds() < 1.0f)
            {
                ColorUtility.TryParseHtmlString("#84130EFF", out myColor);
            }
            else if (timer.getElapsedSeconds() < 2.0f)
            {
                ColorUtility.TryParseHtmlString("#AA6611", out myColor);
            }
            else timer.Reset();
        } else {
            if (timer.getElapsedSeconds() < 1.0f)
            {

                ColorUtility.TryParseHtmlString("#4d6ad8", out myColor);
            }
            else if (timer.getElapsedSeconds() < 2.0f)
            {
                ColorUtility.TryParseHtmlString("#ffffff", out myColor);
            }
            else timer.Reset();
        }

        if (t >= 2.0f) t = 0f;
        else t = Time.deltaTime*2.0f;
        text.color = Color.Lerp(text.color, myColor, t);

    }

    void OnMouseEnter() {
        isOver = true;
    /*
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString("#84130EFF", out myColor);
        text.color = myColor;
*/
    }

    void OnMouseExit() {
        isOver = false;
/*
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString("#4d6ad8", out myColor);
        text.color = myColor;
*/
    }

    void OnDisable() {
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString("#4d6ad8", out myColor);
        text.color = myColor;
        isOver = false;
    }
}
