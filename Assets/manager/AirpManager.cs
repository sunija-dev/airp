using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class AirpManager : MonoBehaviour
{
    public OpenAIAPI m_openAIAPI;
    public TMP_InputField m_input;
    public TMP_InputField m_inputOutput;


    public void Start()
    {
        m_input.text = "This is a nice cake recipe:";
    }

    public void StartGenerate()
    {
        m_inputOutput.text += m_input.text;
        m_openAIAPI.MakeRequest(m_input.text, (string strAnswer) => m_inputOutput.text += strAnswer);
    }

}
