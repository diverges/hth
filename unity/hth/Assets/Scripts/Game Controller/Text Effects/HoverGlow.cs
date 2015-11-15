using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HoverGlow : MonoBehaviour {

    public Text text;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnMouseEnter() {
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString("#84130EFF", out myColor);
        text.color = myColor;
    }

    void OnMouseExit() {
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString("#4d6ad8", out myColor);
        text.color = myColor;
    }

    void OnDisable() {
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString("#4d6ad8", out myColor);
        text.color = myColor;
    }
}
