using UnityEngine;
using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class OpenAIWrapper : MonoBehaviour
{
    [SerializeField] private string basePrompt = "describe this image:";
    [SerializeField] private string openAIKey = "api-key";
    [SerializeField] private int maxTokens = 300;
    [SerializeField, Range(0f, 1f)] private float samplingTemperature = 0.5f; 
    [SerializeField] private ImageDetail imageDetail = ImageDetail.Auto;
    
    public static event Action<string> OnOpenAIResponse;
    public static event Action OnOpenAIRequest;

    public async Task<string> AnalyzeImageWithPrompt(byte[] imageData, string prompt)
    {
        // analyze image with independent prompt
        string base64Image = Convert.ToBase64String(imageData);
        return await MakeOpenAIRequest(base64Image, prompt);
    }
    
    public async Task<string> AnalyzeImage(byte[] imageData)
    {
        // analyze image with specified basePrompt
        string base64Image = Convert.ToBase64String(imageData);
        return await MakeOpenAIRequest(base64Image, basePrompt);
    }

    private async Task<string> MakeOpenAIRequest(string base64Image, string prompt)
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

        string jsonPayload = JsonConvert.SerializeObject(payload);

        var httpResponse = await httpClient.PostAsync(
            "https://api.openai.com/v1/chat/completions", 
            new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

        string jsonResponse = await httpResponse.Content.ReadAsStringAsync();

        if (httpResponse.IsSuccessStatusCode)
        {
            if (jsonResponse.Contains("Error: Unauthorized"))
            {
                // not authorized, aka no api-key?
                return "Error: Unauthorized - check your API key.";
            }
            else
            {
                // otherwise, try to process the successful response
                OnOpenAIResponse?.Invoke(GetResponseContent(jsonResponse));
                return GetResponseContent(jsonResponse);
            }
        }
        return "Error: " + httpResponse.StatusCode.ToString();
    }

    string GetResponseContent(string response)
    {
        try
        {
            var jsonObj = JObject.Parse(response);
            return jsonObj["choices"][0]["message"]["content"].ToString();
        }
        catch (JsonException jsonEx)
        {
            Debug.LogError("JSON Parsing Error: " + jsonEx.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in parsing response: " + ex.Message);
        }
    
        return "Error parsing response: " + response;
    }
}