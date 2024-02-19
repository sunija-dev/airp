using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorkflowDisplayer : MonoBehaviour
{
    private Workflow m_workflow = new Workflow();

    public TMP_Text m_textTitle;
    public Transform m_transStepParent;
    public WorkflowStepDisplayer m_stepDisplayerPrefab;

    private List<WorkflowStepDisplayer> m_liStepDisplayers = new List<WorkflowStepDisplayer>();
    

    public void DisplayWorkflow(Workflow _workflow)
    {
        m_workflow = _workflow;
        m_textTitle.text = m_workflow.strName;

        foreach (WorkflowStepDisplayer stepDisplayer in m_liStepDisplayers)
            Destroy(stepDisplayer.gameObject);

        m_liStepDisplayers.Clear();
        foreach (WorkflowStep step in m_workflow.liSteps)
        { 
            // spawn stepdisplayers
            WorkflowStepDisplayer stepDisplayer = Instantiate(m_stepDisplayerPrefab, m_transStepParent);
            stepDisplayer.DisplayStep(step);
            m_liStepDisplayers.Add(stepDisplayer);
        }
    }

    public void RunWorkflow()
    {
        Debug.Log($"Running workflow {m_workflow.strName}");
        StartCoroutine(ieRunWorkflow());
    }

    public IEnumerator ieRunWorkflow()
    {
        for (int i = 0; i < m_liStepDisplayers.Count; i++)
        {
            Debug.Log($"Workflow step {i} of {m_liStepDisplayers.Count}");
            WorkflowStepDisplayer stepDisplayerCurrent = m_liStepDisplayers[i];

            // create context for that step from previous steps
            string strContext = "";
            if (stepDisplayerCurrent.m_step.bUseContext)
            {
                for (int j = 0; j < i; j++)
                    strContext += m_liStepDisplayers[j].strGetContextInfo();
            }

            yield return stepDisplayerCurrent.coRunStep(strContext);
        }
    }

    public void AddStep()
    {
        Debug.Log($"Adding step to workflow {m_workflow.strName}");
        WorkflowStepDisplayer stepDisplayer = Instantiate(m_stepDisplayerPrefab, m_transStepParent);
        stepDisplayer.DisplayStep(new WorkflowStep());
        m_liStepDisplayers.Add(stepDisplayer);
    }

}
