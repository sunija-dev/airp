using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

[System.Serializable]
public class AiOptions
{
    public string strApiUrl = "http://127.0.0.1:5000/v1/completions";
    public bool bStream = true;


    public int iMaxNewTokens = 500;
    public int iContext = 4096;
    public float fTemperature = 0.7f;
    public float fTopP = 0.9f;
    public float fMinP = 0.15f;
    public bool bFixedSeed = false;
    public int iSeed = 0;
    
    public bool bUseDynaTemperature = false;
    public float fDynaTemperatureLow = 0.7f;
    public float fDynaTemperatureHigh = 1.0f;
    public float fDynaTemperatureExponent = 1f;

    //public bool bUseInstructMode = false;
    //public string str

    public void ApplyToLLMRequest(LLMRequest _request)
    {
        // apply aiOptions to llmRequest
        _request.iMaxTokens = iMaxNewTokens;
        _request.fTemperature = fTemperature;
        _request.fTopP = fTopP;
        _request.fMinP = fMinP;
        _request.iSeed = bFixedSeed ? iSeed : Random.Range(0, int.MaxValue);
        _request.bDynamicTemperature = bUseDynaTemperature;
        _request.fDynaTempLow = fDynaTemperatureLow;
        _request.fDynaTempHigh = fDynaTemperatureHigh;
        _request.bStream = bStream;
    }
}
