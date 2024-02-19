using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class AirpManager : MonoBehaviour
{
    public static AirpManager instance;

    public AiOptions m_aiOptions = new AiOptions();


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Start()
    {
    }

    public void StartGenerate()
    {
        //m_inputOutput.text += m_input.text;
        //MakeRequest(m_input.text, (string strAnswer) => m_inputOutput.text += strAnswer);
    }

    public AiRequest requestMakeRequest(string _strContext, string _strInstruction)
    { 
        AiRequest aiRequest = new AiRequest();
        aiRequest.strContext = _strContext;
        aiRequest.strInstruction = _strInstruction;
        MakeRequest(aiRequest);
        return aiRequest;
    }

    public void MakeRequest(AiRequest _aiRequest)
    {
        _aiRequest.dateStartGeneration = DateTime.Now;
        _aiRequest.eventOnComplete.AddListener((string _strAnswer) =>
        {
            _aiRequest.strAnswer = _strAnswer;
            _aiRequest.dateEndGeneration = DateTime.Now;
        });
        _aiRequest.llmRequest.strPrompt = _aiRequest.strContext + _aiRequest.strInstruction;
        MakeRequest(_aiRequest.llmRequest, _aiRequest.eventOnUpdate.Invoke, _aiRequest.eventOnComplete.Invoke);
    }

    private void MakeRequest(LLMRequest _request, System.Action<string> _actionOnUpdate, System.Action<string> _actionOnComplete)
    {
        m_aiOptions.ApplyToLLMRequest(_request);
        StartCoroutine(OpenAIAPI.ieMakeRequest(_request, m_aiOptions.strApiUrl, _actionOnUpdate, _actionOnComplete));
    }

}

/// <summary>
/// Parent class to keep extra info about llm requests.
/// </summary>
[System.Serializable]
public class AiRequest
{
    public string strGUID = new System.Guid().ToString();
    public DateTime dateCreated = DateTime.Now;

    public bool bFinished = false;

    public LLMRequest llmRequest = new LLMRequest();
    public string strContext = "";
    public string strInstruction = "";
    public string strAnswer = "";

    public DateTime dateStartGeneration;
    public DateTime dateEndGeneration;

    public UnityEvent<string> eventOnUpdate = new UnityEvent<string>();
    public UnityEvent<string> eventOnComplete = new UnityEvent<string>();
}
