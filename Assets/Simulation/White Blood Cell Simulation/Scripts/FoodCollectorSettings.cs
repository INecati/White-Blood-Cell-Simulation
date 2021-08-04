using System;
using UnityEngine;
using UnityEngine.UI;
using MLAgents;

public class FoodCollectorSettings : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] agents;
    [HideInInspector]
    public FoodCollectorArea[] listArea;

    public int totalScore;
    public Text scoreText;

    public void Awake()
    {
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
    }
    int requestNumber = 0;
    public void ResetRequest() {
        requestNumber++;

        if (requestNumber>=20) {
            requestNumber = 0;
            EnvironmentReset();
        }
    }
    public void EnvironmentReset()
    {
        Debug.Log("EnvironmentReset");
        ClearObjects(GameObject.FindGameObjectsWithTag("food"));
        ClearObjects(GameObject.FindGameObjectsWithTag("badFood"));

        agents = GameObject.FindGameObjectsWithTag("agent");
        listArea = FindObjectsOfType<FoodCollectorArea>();
        foreach (var fa in listArea)
        {
            fa.ResetFoodArea(agents);
        }

        totalScore = 0;
    }

    void ClearObjects(GameObject[] objects)
    {
        foreach (var food in objects)
        {
            Destroy(food);
        }
    }

    public void Update()
    {
        scoreText.text = $"Score: {totalScore}";
    }
}
