using System;
using UnityEngine;

public class GPT4Vision : MonoBehaviour
{
    [SerializeField] private OpenAIWrapper openAIWrapper;
    [SerializeField] private PhotoCapturer photoCapturer;

    private void OnEnable()
    {
        if (photoCapturer) photoCapturer.OnPhotoCaptured += OnPhotoCaptured;
        TryGetComponents();
    }
    
    private void OnDisable()
    {
        if (photoCapturer) photoCapturer.OnPhotoCaptured -= OnPhotoCaptured;
    }

    private void TryGetComponents()
    {
        if (!openAIWrapper) openAIWrapper = GetComponentInChildren<OpenAIWrapper>();
        if (!photoCapturer) photoCapturer = GetComponentInChildren<PhotoCapturer>();
    }

    public void CapturePhoto() => photoCapturer.CapturePhoto();

    private async void OnPhotoCaptured(byte[] imageData)
    {
        try
        {
            string response = await openAIWrapper.AnalyzeImage(imageData);
            Debug.Log("OpenAI Response: " + OpenAIWrapper.GetResponseContent(response));
        }
        catch (Exception ex)
        {
            Debug.LogError("Error calling OpenAI: " + ex.Message);
        }
    }
}
