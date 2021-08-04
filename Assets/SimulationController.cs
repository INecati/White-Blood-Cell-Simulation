using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class SimulationController : MonoBehaviour
{
    public GameObject menu;
    public TextMeshProUGUI redText;
    public TextMeshProUGUI plateText;
    public TextMeshProUGUI bacText;
    GameObject[] agents;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartSimulation();
        }
        Debug.Log("update");

        if (Input.GetKeyDown(KeyCode.Escape)) {
            menu.SetActive(!menu.activeSelf);
            
        }
    }
    public void StartSimulation()
    {

        agents = GameObject.FindGameObjectsWithTag("whiteBloodCell");
        foreach (var agent in agents)
        {
            agent.GetComponent<WhiteBloodAgentv1>().AgentReset();
        }
        menu.SetActive(false);
    }
    public void CloseApplication() {
        Application.Quit();
        Debug.Log("Application Closed!");
    }

    //Set Texts
    public void SetredText(float number) {
        redText.text = number.ToString();
    }
    public void SetplateText(float number)
    {
        plateText.text = number.ToString();
    }
    public void SetbacText(float number)
    {
        bacText.text = number.ToString();
    }
}
