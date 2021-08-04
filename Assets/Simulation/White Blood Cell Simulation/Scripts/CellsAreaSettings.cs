using System;
using UnityEngine;
using UnityEngine.UI;
using MLAgents;

public class CellsAreaSettings : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] agents;
    [HideInInspector]
    public CellsArea[] listArea;

    public int totalScore;
    public Text scoreText;
    public int numAgents;
    public void Awake()
    {
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;  //set on
    }
    int requestNumber = 0;
    public void ResetRequest() {
        requestNumber++;

        if (requestNumber>=numAgents) {
            requestNumber = 0;
            EnvironmentReset();
        }
    }
    public void EnvironmentReset()
    {
        Debug.Log("EnvironmentReset");
        ClearObjects(GameObject.FindGameObjectsWithTag("redBloodCell"));
        ClearObjects(GameObject.FindGameObjectsWithTag("bacteria"));
        ClearObjects(GameObject.FindGameObjectsWithTag("platelet"));
        agents = GameObject.FindGameObjectsWithTag("whiteBloodCell");//was agent
        listArea = FindObjectsOfType<CellsArea>();
        foreach (var fa in listArea)
        {
            fa.ResetCellArea(agents);
        }

    }
    void ClearObjects(GameObject[] objects)
    {
        foreach (var cell in objects)
        {
            Destroy(cell);
        }
    }

}
