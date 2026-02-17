using System;
using UnityEngine;
using UnityEngine.UI;

public class BoyVoiceChatGUI : MonoBehaviour
{

    public Text inputText;
    public Text outputText;

    public float fadeOutSpeed = 1f;

    public CanvasGroup inputCanvasGroup;
    public CanvasGroup outputCanvasGroup;
    
    private void Update()
    {
        if(inputCanvasGroup.alpha > 0)
            inputCanvasGroup.alpha = Mathf.Lerp(inputCanvasGroup.alpha, 0, Time.deltaTime * fadeOutSpeed);
        if(outputCanvasGroup.alpha > 0)
            outputCanvasGroup.alpha = Mathf.Lerp(outputCanvasGroup.alpha, 0, Time.deltaTime * fadeOutSpeed);
    }

    
    public void OnResultReceived(string text)
    {
        inputText.text = text;
        inputCanvasGroup.alpha = 1;
    }
    
    public void OnThinking(string text)
    {
        outputText.text = text;
        outputCanvasGroup.alpha = 1;
    }
    
    public void OnSpeechText(string text)
    {
        outputText.text = text;
        outputCanvasGroup.alpha = 1;
    }
    
    
}
