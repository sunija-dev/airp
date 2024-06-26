using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Workflow
{
    public string strGUID = new System.Guid().ToString();
    public string strName = "Workflow";

    public List<WorkflowStep> liSteps = new List<WorkflowStep>();
}

[System.Serializable]
public class WorkflowStep
{
    public string strGUID = new System.Guid().ToString();
    public string strName = "Step";

    public bool bUseContext = true;

    /// <summary>
    /// Prompt used to generate an answer.
    /// </summary>
    public string strPromptGeneration = "";
    /// <summary>
    /// Prompt used to start the text with, to add before the generated answer.
    /// </summary>
    public string strPromptAfterGeneration = "";
    /// <summary>
    /// List of requests/answers if there are variations.
    /// </summary>
    public List<AiRequest> liRequests = new List<AiRequest>();
}




