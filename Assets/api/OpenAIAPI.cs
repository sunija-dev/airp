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

public static class OpenAIAPI
{
    public static IEnumerator ieMakeRequest(LLMRequest _request, string _strApiUrl, System.Action<string> _actionOnUpdate, System.Action<string> _actionOnComplete)
    {
        string strJsonData = JsonConvert.SerializeObject(_request, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
        Debug.Log($"Sending request: \n{strJsonData}");

        using (UnityWebRequest request = new UnityWebRequest(_strApiUrl, "POST"))
        {
            request.SetRequestHeader("accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(strJsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
            float fLastProgress = 0f;

            int iLastIndex = 0;

            // for streaming: check if new tokens arrived
            while (!asyncOperation.isDone)
            {
                float fCurrentProgress = request.downloadProgress;
                // Check if progress has changed
                if (fCurrentProgress != fLastProgress)
                {
                    fLastProgress = fCurrentProgress;
                    string strNewText = request.downloadHandler.text.Substring(iLastIndex);
                    iLastIndex = request.downloadHandler.text.Length;

                    string[] arMessages = strNewText.Split(new char[] { '\n' });
                    List<LLMAnswer> liAnswers = new List<LLMAnswer>();

                    foreach (string strMessage in arMessages)
                    {
                        if (strMessage.Length < 5) // sometimes last line is empty
                            continue;
                        string strMessagePruned = strMessage.Replace("data: ", ""); // TODO: make more performant
                        try
                        {
                            LLMAnswer llmAnswer = JsonConvert.DeserializeObject<LLMAnswer>(strMessagePruned);
                            _actionOnUpdate?.Invoke(llmAnswer.liChoices[0].strText);
                            //Debug.Log($"Updated request with {llmAnswer.liChoices[0].strText}");
                        }
                        catch (System.Exception e)
                        {
                            Debug.Log($"Error: {e.Message}");
                        }
                    }
                }
                yield return null;
            }

            if (request.result != UnityWebRequest.Result.Success)
                throw new System.Exception(request.error);

            // without streaming: take message after it arrived in full
            if (_request.bStream != true)
            {
                Debug.Log("end: " + request.downloadHandler.text);
                LLMAnswer llmAnswer = JsonConvert.DeserializeObject<LLMAnswer>(request.downloadHandler.text);
                _actionOnComplete?.Invoke(llmAnswer.liChoices[0].strText);
            }
            else
            {
                _actionOnComplete?.Invoke("");
            }
        }
    }
}



[System.Serializable]
public class LLMRequest
{
    [JsonProperty("model")]
    public string strModel;

    [JsonProperty("prompt")]
    public string strPrompt;

    //[JsonProperty("best_of")]
    //public int iBestOf; // not used by ooba

    [JsonProperty("echo")]
    public bool? bEcho;

    [JsonProperty("frequency_penalty")]
    public float? fFrequencyPenalty;

    [JsonProperty("logit_bias")]
    public object? logitBias; // dictionary

    [JsonProperty("logprobs")]
    public int? logProbs;

    [JsonProperty("max_tokens")]
    public int? iMaxTokens = 50;

    //[JsonProperty("n")]
    //public int iN; // not used by ooba

    [JsonProperty("presence_penalty")]
    public float? fPresencePenalty;

    [JsonProperty("stop")]
    public List<string> strStop = new List<string>();

    [JsonProperty("stream")]
    public bool? bStream;

    [JsonProperty("suffix")]
    public string strSuffix;

    [JsonProperty("temperature")]
    public float? fTemperature;

    [JsonProperty("top_p")]
    public float? fTopP;

    [JsonProperty("user")]
    public string strUser;

    //[JsonProperty("preset")]
    //public string strPreset; // instruction preset, not used by us

    [JsonProperty("min_p")]
    public float? fMinP;

    [JsonProperty("dynamic_temperature")]
    public bool? bDynamicTemperature;

    [JsonProperty("dynatemp_low")]
    public float? fDynaTempLow;

    [JsonProperty("dynatemp_high")]
    public float? fDynaTempHigh;

    [JsonProperty("dynatemp_exponent")]
    public float? fDynaTempExponent;

    [JsonProperty("top_k")]
    public int? iTopK;

    [JsonProperty("repetition_penalty")]
    public float? repetitionPenalty;

    [JsonProperty("repetition_penalty_range")]
    public int? repetitionPenaltyRange;

    [JsonProperty("typical_p")]
    public float? typicalP;

    [JsonProperty("tfs")]
    public float? fTfs;

    [JsonProperty("top_a")]
    public float? fTopA;

    [JsonProperty("epsilon_cutoff")]
    public int? epsilonCutoff;

    [JsonProperty("eta_cutoff")]
    public float? fEtaCutoff;

    [JsonProperty("guidance_scale")]
    public float? fGuidanceScale;

    [JsonProperty("negative_prompt")]
    public string strNegativePrompt;

    [JsonProperty("penalty_alpha")]
    public float? fPenaltyAlpha;

    [JsonProperty("mirostat_mode")]
    public int? miroStatMode;

    [JsonProperty("mirostat_tau")]
    public float? fMiroStatTau;

    [JsonProperty("mirostat_eta")]
    public float? fMiroStatEta;

    [JsonProperty("temperature_last")]
    public bool? bTemperatureLast;

    [JsonProperty("do_sample")]
    public bool? bDoSample;

    [JsonProperty("seed")]
    public int? iSeed;

    [JsonProperty("encoder_repetition_penalty")]
    public float? fEncoderRepetitionPenalty;

    [JsonProperty("no_repeat_ngram_size")]
    public int? iNoRepeatNgramSize;

    [JsonProperty("min_length")]
    public int? iMinLength;

    [JsonProperty("num_beams")]
    public int? iNumBeams;

    [JsonProperty("length_penalty")]
    public float? fLengthPenalty;

    [JsonProperty("early_stopping")]
    public bool? bEarlyStopping;

    [JsonProperty("truncation_length")]
    public int? iTruncationLength;

    [JsonProperty("max_tokens_second")]
    public int? iMaxTokensSecond;

    [JsonProperty("prompt_lookup_num_tokens")]
    public int? iPromptLookupNumTokens;

    [JsonProperty("custom_token_bans")]
    public string? strCustomTokenBans;

    [JsonProperty("auto_max_new_tokens")]
    public bool? bAutoMaxNewTokens;

    [JsonProperty("ban_eos_token")]
    public bool? bBanEOSToken;

    [JsonProperty("add_bos_token")]
    public bool? bAddBOSToken;

    [JsonProperty("skip_special_tokens")]
    public bool? bSkipSpecialTokens;

    [JsonProperty("grammar_string")]
    public string strGrammarString;
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
