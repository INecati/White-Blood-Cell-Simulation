using UnityEngine;
using MLAgentsExamples;

public class CellsArea : Area
{
    public GameObject redBloodCell;
    public GameObject bacteria;
    public GameObject platelet;
    public int numRedBloodCell;
    public int numBacteria;
    public int numPlatelet;
    public bool respawnCell;
    public float range;

    void CreateCell(int num, GameObject type)
    {
        /*
        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(type, new Vector3(Random.Range(-range, range), 1f,
                Random.Range(-range, range)) + transform.position,
                Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 90f)));
            f.GetComponent<CellLogic>().respawn = respawnCell;
            f.GetComponent<CellLogic>().myArea = this;
        }*/
        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(type, new Vector3(Random.Range(-range, range), Random.Range(-range, range),0) + transform.position,
                Quaternion.Euler(new Vector3(0f,0f , Random.Range(0f, 360f))));
            f.GetComponent<CellLogic>().respawn = respawnCell;
            f.GetComponent<CellLogic>().myArea = this;
        }
    }

    public void ResetCellArea(GameObject[] agents)
    {
        Debug.Log("area reseted");
        foreach (GameObject agent in agents)
        {
            if (agent.transform.parent == gameObject.transform)
            {
                agent.transform.position = new Vector3(Random.Range(-range, range), 2f,
                    Random.Range(-range, range))
                    + transform.position;
                //new
                agent.transform.position = new Vector3(Random.Range(-range, range), Random.Range(-range, range),0)+ transform.position;
                //agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
            }
        }
        // Convert.ToInt32(vIn);
        CreateCell(numRedBloodCell, redBloodCell);
        CreateCell(numBacteria, bacteria);
        CreateCell(numPlatelet,platelet);
    }

    public override void ResetArea()
    {
        Debug.Log("area reseted");
    }
    public void SetRedBloodCellNumber(float num) {
        numRedBloodCell = (int)num;
    }
    public void SetPlateletNumber(float num)
    {
        numPlatelet = (int)num;
    }
    public void SetBacteriaNumber(float num)
    {
        numBacteria = (int)num;
    }
}
