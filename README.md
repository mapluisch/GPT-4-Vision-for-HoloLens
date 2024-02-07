# GPT-4 Vision for HoloLens

## Overview
This project demonstrates the integration of OpenAI's GPT-4 Vision API with a HoloLens application. Users can capture images using the HoloLens camera and receive descriptive responses from the GPT-4V model. 

### Demo
https://github.com/mapluisch/GPT-4-Vision-for-HoloLens/assets/31780571/03260bce-97c2-481d-b0e8-6c04e4cf496d

## Dependencies
- Newtonsoft.JSON
- MRTK Foundation
- MRTK Standard Assets

## Setup
1. Open the `GPT4 Vision Example`-Scene
2. Specify your OpenAI key in the GameObject `GPT4Vision` > `OpenAIWrapper` (or hardcode it into the OpenAIWrapper.cs class)
3. Specify your base prompt (which is concatenated to the image sent to OpenAI), e.g. <i>Describe this image.</i>
4. Specify max tokens, sampling temperature, and image detail for the OpenAI API call

### Running the application
1. Build the app as `.appx` (or deploy to HoloLens directly, e.g. via Visual Studio) and install it on your HoloLens
2. Run the app. Press on the camera button to capture a photo using HoloLens' PV camera which gets send to OpenAI's API.
3. See the inference result (based on your prompt) displayed on the label.

### Using the .unitypackage
1. Make sure you have the dependencies from above installed.
2. Import the package via `Assets > Import Package`.
3. Either open up the `GPT4 Vision Example`-Scene, or import the `GPT4Vision`-Prefab into your own scene.
4. Edit the base prompt, tokens, temperature, image detail as described above.
5. Optional: call `CapturePhoto()` within the `GPT4Vision`-Prefab (in case you do not want to use the button and label within the Prefab).

## Performance improvements
For some reason, the built-in `UnityEngine.Windows.WebCam` approach provided by Microsoft is really slow (~1.2s per captured photo on average, regardless of resolution). Also, inference speed on OpenAI's server can vary quite a bit. If you need this approach in real-time, skip `PhotoCapture` altogether (Research Mode) and think about hosting your own LMM. Feel free to message me if you need some pointers.

## Disclaimer
This project is a barebones prototype for now and still WIP. Feel free to create a PR.
