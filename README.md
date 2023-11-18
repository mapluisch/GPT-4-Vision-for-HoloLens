# GPT-4 Vision Integration for HoloLens

## Overview
This project demonstrates the integration of OpenAI's GPT-4 Vision API with a HoloLens application. Users can capture images using the HoloLens camera and receive descriptive responses from the GPT-4V model. Alternatively, users can import and specify images (tested with .jpg) and receive GPT-4V responses as well.

This app uses Unity 2022.3.4f1, although newer versions should work fine (untested, though!).

### Setup
1. Open the <i>GPT4VisionExample</i>-Scene
2. Specify your OpenAI key in the GameObject <i>GPT4Vision</i> > <i>OpenAIWrapper</i> (or hardcode it into the OpenAIWrapper.cs class)
3. Specify your base prompt (which is concatenated to the image sent to OpenAI), e.g. <i>describe this image:</i>
4. Specify max tokens, sampling temperature, and image detail for the OpenAI API call

### Running the application
When running the application within the editor, the GameObject <i>ImageTest</i> will send an examplatory image stored in the <i>Images</i> folder to GPT-4V and asks for an image description (which then is printed to the console after a couple seconds).

If you want to capture and use the photos from a HoloLens directly, disable the ImageTest GameObject, and simply link the function call of <i>GPT4Vision.cs</i> > <i>CapturePhoto</i> (e.g., to a HoloLens button, finger gesture, ...).

## Disclaimer
This project is a barebones prototype for now and still WIP. Better built-in functionalities will come, potentially adding standard GPT-4 requests / GPT-4-Audio as well. Feel free to create a PR.