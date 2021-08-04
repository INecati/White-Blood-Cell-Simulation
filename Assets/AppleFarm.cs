using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleFarm : MonoBehaviour
{
    public int treeNumber;
    public float range;

    public GameObject appleTree;


    public void ResetArea() {

        ClearArea();
        ResetTrees();
    }
    public void ClearArea() {

        foreach (Transform child in transform)
        {
            if (child.name == "Apple Tree" || child.name == "Pillar(Clone)")
            {
                //Debug.Log(child.name);
                //Destroy(child.gameObject);
            }
        }
    }
    public void ResetTrees()
    {

        foreach (Transform child in transform) {

            if (child.CompareTag("tree")) {
                child.GetComponent<AppleTree>().ResetTree();
            }
        }
    }
}
