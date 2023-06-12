using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextControl : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI adjustableText;

    [Range(0, 1)]
    [SerializeField] float alphaAmount = 0.5f;

    private void Start()
    {
        StartCoroutine(GameStartedText());
    }

    public IEnumerator GameStartedText()
    {
        adjustableText.text = "GAME IS STARTING !";

        adjustableText.gameObject.active = true;
        Time.timeScale = 0.2f;       

        adjustableText.fontSize = 90f;
        yield return new WaitForSecondsRealtime(0.25f);
        adjustableText.fontSize = 125f;
        yield return new WaitForSecondsRealtime(0.25f);
        adjustableText.fontSize = 90f;
        yield return new WaitForSecondsRealtime(0.25f);
        adjustableText.fontSize = 125f;
        yield return new WaitForSecondsRealtime(0.25f);
        adjustableText.fontSize = 90f;
        yield return new WaitForSecondsRealtime(0.25f);
        adjustableText.fontSize = 125f;
        yield return new WaitForSecondsRealtime(0.25f);

        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(0.5f);
        adjustableText.gameObject.active = false; 
    }

    public void StartGameOverText()
    {
        StartCoroutine(GameOverTextAnimation());
    }

    public IEnumerator GameOverTextAnimation()
    {
        adjustableText.gameObject.SetActive(true);
        //Definitions
        adjustableText.text = "GAME OVER";
        var textColor = adjustableText.color;
        //Configurations
        Time.timeScale = 0.2f;

        for(int i = 0; i < 15; i++)
        {
            textColor.a = alphaAmount;
            adjustableText.color = textColor;
            yield return new WaitForSecondsRealtime(0.1f);
            adjustableText.color = Color.white;
            textColor.a = 1f;
            adjustableText.color = textColor;
            yield return new WaitForSecondsRealtime(0.1f);
            textColor.a = alphaAmount;
            adjustableText.color = textColor;
            yield return new WaitForSecondsRealtime(0.1f);
            textColor.a = 1f;
            adjustableText.color = textColor;
            adjustableText.color = Color.red;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        yield return new WaitForSecondsRealtime(1.5f);
        adjustableText.color = Color.white;
        Time.timeScale = 1f;
        adjustableText.gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);
        FindObjectOfType<Fader>().FadeOut(2f);
    }
}
