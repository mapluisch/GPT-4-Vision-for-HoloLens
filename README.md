# GPT-4 Vision for HoloLens

## Overview
This project demonstrates the integration of OpenAI's GPT-4 Vision API with a HoloLens application. Users can capture images using the HoloLens camera and receive descriptive responses from the GPT-4V model. Alternatively, users can import and specify image files (tested with .jpg) and receive GPT-4V responses.

This app uses Unity 2022.3.4f1, although newer versions should work fine (untested, though!).

### Dependencies
- Newtonsoft.Json (used for parsing OpenAI's response object, so somewhat optional)

### Setup
1. Open the `GPT4VisionExample`-Scene
2. Specify your OpenAI key in the GameObject `GPT4Vision` > `OpenAIWrapper` (or hardcode it into the OpenAIWrapper.cs class)
3. Specify your base prompt (which is concatenated to the image sent to OpenAI), e.g. <i>describe this image:</i>
4. Specify max tokens, sampling temperature, and image detail for the OpenAI API call

### Running the application
When running the application within the editor, the GameObject `ImageTest` will send an examplatory image stored in the `Images` folder to GPT-4V and asks for an image description (which then is printed to the console after a couple seconds).

If you want to capture and use the photos from a HoloLens directly, disable the ImageTest GameObject, and simply link the function call of `GPT4Vision.cs` > `CapturePhoto` (e.g., to a HoloLens button, finger gesture, ...). Please be aware that capturing images with the HoloLens only works on a real device - simulator is not supported. 

You can call the `OpenAIWrapper` function `AnalyzeImageWithPrompt` with any image as `byte[]` and specify your own base prompt on call as well. Just link a ref to any of your own scripts and it <i>should</i> work. 

## Disclaimer
This project is a barebones prototype for now and still WIP. Feel free to create a PR.
