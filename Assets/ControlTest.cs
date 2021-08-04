using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTest : MonoBehaviour
{
    Rigidbody2D m_AgentRb;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        m_AgentRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Move();
        
    }
    private void Move()
    {
        var verDir=Vector2.zero;
        var horDir=Vector2.zero;
        if (Input.GetKey(KeyCode.W)) {
            verDir = transform.forward;
        }
        

        m_AgentRb.AddForce(verDir * moveSpeed);
        m_AgentRb.AddForce(horDir * moveSpeed);
    }
}
