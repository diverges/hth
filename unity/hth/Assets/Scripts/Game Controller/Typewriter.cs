using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour {

    public float delay = 0.2f;
    public string message = "";

    public Text guiText;

    public bool typing = false;

	// Use this for initialization
	void Start () {
        if (guiText != null)
        {
            message = guiText.text;
            guiText.text = "";
        }
	}

    public void LoadText(string message) {
        if (typing) return;

        typing = true;
        guiText.text = "";
        this.message = message;
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        typing = true;
        foreach (char letter in message.ToCharArray())
        {
            guiText.text += letter;
            yield return new WaitForSeconds (delay);
        }
        typing = false;
    }
}
