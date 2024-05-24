using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkflowStepDisplayer : MonoBehaviour
{
    public WorkflowStep m_step = new WorkflowStep();

    public TMP_InputField m_inputTitle;
    public TMP_InputField m_inputPromptGeneration;
    public TMP_InputField m_inputPromptAfterGeneration;
    public TMP_InputField m_inputAnswer;

    private int m_iRequestCurrent = -1;
    private AiRequest m_airequestCurrent = null;
    //private Coroutine m_coCurrentStep = null;

    public void DisplayStep(WorkflowStep _step)
    {
        m_step = _step;
        m_inputTitle.text = m_step.strName;
        m_inputPromptGeneration.text = m_step.strPromptGeneration;
        m_inputPromptAfterGeneration.text = m_step.strPromptAfterGeneration;

        if (m_step.liRequests.Count > 0)
            DisplayRequest(0);
    }

    public void DisplayRequest(int _iIndex)
    {
        if (_iIndex < 0 || _iIndex >= m_step.liRequests.Count)
            return;
        m_iRequestCurrent = _iIndex;

        m_airequestCurrent = m_step.liRequests[_iIndex];

        // show full text, if it is there
        if (m_airequestCurrent.bFinished)
            m_inputAnswer.text = m_airequestCurrent.strAnswer;
        // ...otherwise show updates
        m_airequestCurrent.eventOnUpdate.AddListener(OnAnswerUpdate);
        Debug.Log($"Displaying request {_iIndex} of {m_step.liRequests.Count}");
    }

    public void OnAnswerUpdate(string _strUpdate)
    {
        if (m_iRequestCurrent >= 0)
            m_inputAnswer.text += _strUpdate;
    }

    /*
    public Coroutine coRunStep(string _strContext)
    {
        
        m_coCurrentStep = StartCoroutine(ieRunStep(_strContext));
        return m_coCurrentStep;
    }
    */

    public IEnumerator ieRunStep(string _strContext)
    {
        Debug.Log($"Running step {m_step.strName}");
        UpdateDataValues();

        // create request if there is none yet
        if (m_iRequestCurrent < 0)
        {
            AiRequest aiRequest = AirpManager.instance.requestMakeRequest(_strContext, m_step.strPromptGeneration);
            m_step.liRequests.Add(aiRequest);
            DisplayRequest(0);
        }

        m_airequestCurrent.strContext = _strContext;
        m_airequestCurrent.strInstruction = m_step.strPromptGeneration;

        //AirpManager.instance.MakeRequest(m_airequestCurrent);
        yield return new WaitUntil(() => m_airequestCurrent.bFinished);

        m_iRequestCurrent = -1; // TODO: no idea what i wanted to do with that var before
    }

    public void UpdateDataValues()
    { 
        m_step.strName = m_inputTitle.text;
        m_step.strPromptGeneration = m_inputPromptGeneration.text;
        m_step.strPromptAfterGeneration = m_inputPromptAfterGeneration.text;
    }

    public string strGetContextInfo()
    { 
        // only creates context if the answer is not empty
        if (m_airequestCurrent == null || string.IsNullOrEmpty(m_airequestCurrent.strAnswer))
            return "";

        string strContext = m_step.strPromptAfterGeneration + m_airequestCurrent.strAnswer;
        return strContext;
    }
}
