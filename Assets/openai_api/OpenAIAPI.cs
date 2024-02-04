using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using UnityEngine.Networking;

/// <summary>
/// http://127.0.0.1:5000/docs
/// </summary>

public class OpenAIAPI : MonoBehaviour
{
    private string strApiUrl = "http://127.0.0.1:5000/v1/completions";

    public void MakeRequest(string _strPrompt, System.Action<string> _actionOnComplete)
    {
        StartCoroutine(ieMakeRequest(_strPrompt, _actionOnComplete));
    }

    IEnumerator ieMakeRequest(string _strPrompt, System.Action<string> _actionOnComplete)
    {
        LLMRequest requestBody = new LLMRequest()
        {
            strPrompt = _strPrompt,
            iMaxTokens = 50,
            fTemperature = 0.7f,
            fTopP = 0.9f,
            iSeed = Random.Range(0, int.MaxValue),
        };

        string strJsonData = JsonConvert.SerializeObject(requestBody, Formatting.Indented);
        Debug.Log($"Sending request: \n{strJsonData}");

        using (UnityWebRequest request = new UnityWebRequest(strApiUrl, "POST"))
        {
            request.SetRequestHeader("accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(strJsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();


            // Start the request asynchronously
            UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
            float fLastProgress = 0f;

            // Continue updating progress until the request is completed
            while (!asyncOperation.isDone)
            {
                float fCurrentProgress = request.downloadProgress;
                // Check if progress has changed
                if (fCurrentProgress != fLastProgress) //&& callback != null)
                {
                    //callback?.Invoke(ConvertContent(request.downloadHandler.text, getContent));
                    fLastProgress = fCurrentProgress;
                    Debug.Log(request.downloadHandler.text);
                }
                yield return null;
            }

            if (request.result != UnityWebRequest.Result.Success)
                throw new System.Exception(request.error);

            Debug.Log(request.downloadHandler.text);
            LLMAnswer llmAnswer = JsonConvert.DeserializeObject<LLMAnswer>(request.downloadHandler.text);
            _actionOnComplete?.Invoke(llmAnswer.liChoices[0].strText);
        }
        
    }
}

[System.Serializable]
public class LLMRequest
{
    [JsonProperty("prompt")]
    public string strPrompt = "";
    //[JsonProperty("stream")]
    //public bool bStream = true;
    [JsonProperty("max_tokens")]
    public int iMaxTokens = 50;
    [JsonProperty("temperature")]
    public float fTemperature = 0.7f;
    [JsonProperty("top_p")]
    public float fTopP = 0.9f;
    [JsonProperty("min_p")]
    public float fMinP = 0.15f;
    [JsonProperty("seed")]
    public int iSeed = Random.Range(0, int.MaxValue);
}

[System.Serializable]
public class LLMAnswer
{
    [JsonProperty("id")]
    public string strId = "";
    [JsonProperty("object")]
    public string strObject = "";
    [JsonProperty("created")]
    public int iCreated = -1;
    [JsonProperty("model")]
    public string strModel = "";
    [JsonProperty("choices")]
    public List<LLMChoice> liChoices = new List<LLMChoice>();
}

[System.Serializable]
public class LLMChoice
{
    [JsonProperty("index")]
    public int iId = -1;
    [JsonProperty("finish_reason")]
    public string strFinishReason = "";
    [JsonProperty("text")]
    public string strText;
}

[System.Serializable]
public class LLMUsage
{
    [JsonProperty("prompt_tokens")]
    public int iPromptTokens = -1;
    [JsonProperty("completion_tokens")]
    public int iCompletionTokens = -1;
    [JsonProperty("total_tokens")]
    public int iTotalTokens = -1;
}
