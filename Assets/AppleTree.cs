using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    public int appleNumber = 5;
    public float durability = 100;
    public GameObject apple;
    public GameObject[] apples;
    private Vector3[] applePositions;
    private void Start()
    {
        applePositions = new Vector3[5];
        for (int i = 0; i < 5; i++) {
            applePositions[i] = apples[i].transform.position;
        }
    }

    public int dealDamage(float damage) {
        int droppedApples = 0;
        durability -= damage;
        //(appleNumber-1)*20=>durability
        
        while ( (appleNumber-1)*20>=durability && appleNumber>0 ) {

            dropApple();
            droppedApples++;
        }
        return droppedApples;
    }
    public void dropApple() {
        appleNumber--;
        //Instantiate(apple);
        GameObject apple;
        apple = apples[appleNumber];
        apples[appleNumber] = null;
        apple.GetComponent<Rigidbody>().useGravity=true;
        apple.GetComponent<SphereCollider>().enabled = true;
        //Debug.Log("Apple Dropped");
        //dropapple
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("agent")) {
            //Debug.Log("relativeVelocity: "+collision.relativeVelocity.ToString());
            //Debug.Log("speed:" + Vector3.Distance(Vector3.zero, collision.rigidbody.velocity));
        }
    }
    public void ResetTree() {
        appleNumber = 5;
        durability = 100;

        foreach (Transform child in transform)
        {
            if (child.CompareTag("apple"))
            {
                //Debug.Log(child.name);
                Destroy(child.gameObject);
            }
        }

        for (int i=0;i<5;i++) {
            
            apples[i]= Instantiate(apple, applePositions[i], Quaternion.identity, transform);
        }
    }
}
