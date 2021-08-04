using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] objs;
    public CellsAreaSettings cellsAreaSetting;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        Debug.Log("fixedupdate");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            StartSimulation();
        }
        Debug.Log("update");
    }
    void StartSimulation() {

        //foreach (GameObject obj in objs) {
        //    obj.SetActive(true);
        //}

        cellsAreaSetting.EnvironmentReset();
        Time.timeScale = 0.01f;
    }
}
