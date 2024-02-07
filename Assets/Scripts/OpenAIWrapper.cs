using System;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;

public class OpenAIWrapper : MonoBehaviour
{
    [SerializeField] private string basePrompt = "describe this image:";
    [SerializeField] private string openAIKey = "api-key";
    [SerializeField] private int maxTokens = 300;
    [SerializeField, Range(0f, 1f)] private float samplingTemperature = 0.5f; 
    [SerializeField] private ImageDetail imageDetail = ImageDetail.Auto;
    
    public static event Action<string> OnOpenAIResponse;
    public static event Action OnOpenAIRequest;

    public async Task AnalyzeImage(byte[] imageData, string prompt = "")
    {
        string base64Image = Convert.ToBase64String(imageData);
        await MakeOpenAIRequest(base64Image, String.IsNullOrEmpty(prompt) ? basePrompt : prompt);
    }
    
    // sends an API req to OpenAI containing the captured image as b64-string and a user prompt
    private async Task MakeOpenAIRequest(string base64Image, string prompt)
    {
        Debug.Log("Sending new request to OpenAI.");
        OnOpenAIRequest?.Invoke();

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAIKey);
            
        // via https://platform.openai.com/docs/guides/vision
        var payload = new
        {
            model = "gpt-4-vision-preview",
            messages = new object[]
            {
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new
                        {
                            type = "text",
                            text = prompt
                        },
                        new
                        {
                            type = "image_url",
                            image_url = new
                            {
                                url = "data:image/jpeg;base64," + base64Image,
                                detail = imageDetail.ToString().ToLower()
                            }
                        }
                    }
                }
            },
            max_tokens = maxTokens,
            temperature = samplingTemperature
        };

        var httpResponse = await httpClient.PostAsync(
            "https://api.openai.com/v1/chat/completions", 
            new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

        string jsonResponse = await httpResponse.Content.ReadAsStringAsync();

        if (httpResponse.IsSuccessStatusCode)
        {
            if (jsonResponse.Contains("Error: Unauthorized")) Debug.Log("Error: Unauthorized - check your API key.");
            else
            {
                OnOpenAIResponse?.Invoke(GetResponseContent(jsonResponse));
                return;
            }
        }
        Debug.Log("Error: " + httpResponse);
    }
    
    // function to parse the response JSON and return the actual response message from OpenAI
    string GetResponseContent(string response)
    {
        try
        {
            var jsonObj = JObject.Parse(response);
            return jsonObj["choices"][0]["message"]["content"].ToString();
        }
        catch (JsonException jsonEx)
        {
            Debug.Log("JSON Parsing Error: " + jsonEx.Message);
        }
        catch (Exception ex)
        {
            Debug.Log("Error in parsing response: " + ex.Message);
        }
    
        return "Error parsing response: " + response;
    }
}