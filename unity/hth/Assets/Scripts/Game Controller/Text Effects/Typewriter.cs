using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Typewriter : MonoBehaviour {

    public float delay = 0.2f;
    public string message = "";
    private Queue<IEnumerator> type_queue;

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
        type_queue = new Queue<IEnumerator>();
	}

    public void Clear() {
        StopCoroutine(co);
        typing = false;
    }

    public void SetText(string message) {
        this.guiText.text = message;
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

        co = StartCoroutine(TypeText(message, d));
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

    public void AppendText(string message, float d)
    {
        if(typing) {
            this.type_queue.Enqueue(TypeText(message, d));
        } else { 
            typing = true;
            this.message = message;
            co = StartCoroutine(TypeText(message, d));
        }
    }

    public void AppendText(string message, AudioSource v, float d)
    {
        if (typing)
        {
            this.type_queue.Enqueue(TypeText(message, v, d));
        }
        else
        {
            typing = true;
            this.message = message;
            co = StartCoroutine(TypeText(message, v, d));
        }
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
        if (type_queue.Count != 0) yield return StartCoroutine(type_queue.Dequeue());
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
        if (type_queue.Count != 0) yield return StartCoroutine(type_queue.Dequeue());
        typing = false;
    }

    IEnumerator TypeText(string message, float d)
    {
        typing = true;
        foreach (char letter in message.ToCharArray())
        {
            guiText.text += letter;
            yield return new WaitForSeconds(d);
        }
        if (type_queue.Count != 0) yield return StartCoroutine(type_queue.Dequeue());
        typing = false;
    }

    IEnumerator TypeText(string message, AudioSource v, float d)
    {
        typing = true;
        foreach (char letter in message.ToCharArray())
        {
            guiText.text += letter;
            v.Play();
            yield return new WaitForSeconds(d);
        }
        if (type_queue.Count != 0) yield return StartCoroutine(type_queue.Dequeue());
        typing = false;
    }
}
