using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Windows.WebCam;

public class PhotoCapturer : MonoBehaviour
{
    private PhotoCapture photoCaptureObject = null;
    private CameraParameters cameraParameters;
    private bool isInPhotoMode = false;
    public event Action<byte[]> OnPhotoCaptured;

    private void OnEnable()
    {
        StartPhotoCaptureProcess();
    }

    private void OnDisable()
    {
        EndPhotoCaptureProcess(); 
    }

    private void StartPhotoCaptureProcess() => PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
    
    private void EndPhotoCaptureProcess() => photoCaptureObject?.StopPhotoModeAsync(OnStoppedPhotoMode);
    
    private void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        photoCaptureObject = captureObject;
        cameraParameters = new CameraParameters
        {
            hologramOpacity = 0.0f,
            cameraResolutionWidth = 1280,
            cameraResolutionHeight = 720,
            pixelFormat = CapturePixelFormat.JPEG
        };

        photoCaptureObject.StartPhotoModeAsync(cameraParameters, OnPhotoModeStarted);
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            isInPhotoMode = true;
            CapturePhoto();
        }
        else
        {
            Debug.LogError("Failed to start photo mode.");
        }
    }

    public void CapturePhoto()
    {
        if(isInPhotoMode)
            photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
        else Debug.Log("Photo mode not ready, please re-init.");
    }

    private void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            List<byte> imageDataBuffer = new List<byte>();
            photoCaptureFrame.CopyRawImageDataIntoBuffer(imageDataBuffer);
            
            byte[] imageData = imageDataBuffer.ToArray();
            OnPhotoCaptured?.Invoke(imageData);
        }
        else
        {
            Debug.LogError("Failed to capture photo.");
        }
    }

    
    private void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        isInPhotoMode = false;
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }
}
