using Newtonsoft.Json.Linq;
using UnityEngine;

public class ImageTest : MonoBehaviour
{
    [SerializeField] private Texture2D imageToAnalyze;
    [SerializeField] private OpenAIWrapper openAIWrapper; 

    async void Start()
    {
        if (imageToAnalyze != null)
        {
            Texture2D uncompressedTexture = ConvertToUncompressed(imageToAnalyze);

            byte[] imageData = uncompressedTexture.EncodeToJPG();
            string response = await openAIWrapper.AnalyzeImage(imageData);
            
            Debug.Log("OpenAI response: " + response);
        }
        else
        {
            Debug.LogError("No image assigned in the Editor.");
        }
    }

    private Texture2D ConvertToUncompressed(Texture2D source)
    {
        Texture2D uncompressed = new Texture2D(source.width, source.height);
        uncompressed.SetPixels(source.GetPixels());
        uncompressed.Apply();
        return uncompressed;
    }
}