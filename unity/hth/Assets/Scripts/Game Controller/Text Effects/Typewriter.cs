using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour {

    public float delay = 0.2f;
    public string message = "";

    public Text guiText;

    public bool typing = false;
    public bool fading = false;

    private Coroutine co;

	// Use this for initialization
	void Start () {
        if (guiText != null)
        {
            message = guiText.text;
            guiText.text = "";
        }
	}

    public void Clear() {
        StopCoroutine(co);
        typing = false;
    }

    public void LoadText(string message) {
        typing = true;
        guiText.text = "";
        this.message = message;

        // Reset alpha
        guiText.material.color = new Color(
                   guiText.font.material.color.r,
                   guiText.font.material.color.g,
                   guiText.font.material.color.b, 
                   1.0f);

        co = StartCoroutine(TypeText());
    }

    public void LoadText(string message, float d)
    {
        typing = true;
        guiText.text = "";
        this.message = message;

        // Reset alpha
        guiText.material.color = new Color(
                   guiText.font.material.color.r,
                   guiText.font.material.color.g,
                   guiText.font.material.color.b,
                   1.0f);

        co = StartCoroutine(TypeText(d));
    }

    public void LoadText(string message, AudioSource voice)
    {
        typing = true;
        guiText.text = "";
        this.message = message;

        // Reset alpha
        guiText.material.color = new Color(
                   guiText.font.material.color.r,
                   guiText.font.material.color.g,
                   guiText.font.material.color.b,
                   1.0f);

        co = StartCoroutine(TypeText(voice));
    }

    public void Fade(float duration) {
        fading = true;
        co = StartCoroutine(FadeOut(duration));
    }

    IEnumerator FadeOut(float duration) {
        fading = true;
        float alpha = guiText.material.color.a;
        float interval = 1.0f / duration;
        float a;

        for(float t = 0; t < 1.0f; t += Time.deltaTime*interval) {
            a = Mathf.Lerp(1.0f, 0.0f, t);
            guiText.material.color = new Color(
                guiText.font.material.color.r,
                guiText.font.material.color.g,
                guiText.font.material.color.b, a);
            yield return 0;
        }
        fading = false;
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

    IEnumerator TypeText(AudioSource v)
    {
        typing = true;
        foreach (char letter in message.ToCharArray())
        {
            guiText.text += letter;
            v.Play();
            yield return new WaitForSeconds(delay);
        }
        typing = false;
    }

    IEnumerator TypeText(float d)
    {
        typing = true;
        foreach (char letter in message.ToCharArray())
        {
            guiText.text += letter;
            yield return new WaitForSeconds(d);
        }
        typing = false;
    }
}
