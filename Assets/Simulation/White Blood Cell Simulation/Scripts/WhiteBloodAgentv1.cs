using UnityEngine;
using MLAgents;

public class WhiteBloodAgentv1 : Agent
{
    CellsAreaSettings m_CellsAreaSettings;
    public GameObject area;
    CellsArea m_MyArea;
    bool m_Eat;
    Rigidbody2D m_AgentRb;
    public float turnSpeed = 300;

    // Speed of agent movement.
    public float moveSpeed = 2;
    public Material normalMaterial;
    public Material badMaterial;
    public Material goodMaterial;
    public Material frozenMaterial;

    public bool contribute;
    public bool useVectorObs;


    public override void InitializeAgent()
    {
        base.InitializeAgent();
        m_AgentRb = GetComponent<Rigidbody2D>();
        Monitor.verticalOffset = 1f;
        m_MyArea = area.GetComponent<CellsArea>();
        m_CellsAreaSettings = FindObjectOfType<CellsAreaSettings>();
        Debug.Log("initialized");
    }

    public override void CollectObservations()
    {
        if (useVectorObs)
        {

        }
    }

    public Color32 ToColor(int hexVal)
    {
        var r = (byte)((hexVal >> 16) & 0xFF);
        var g = (byte)((hexVal >> 8) & 0xFF);
        var b = (byte)(hexVal & 0xFF);
        return new Color32(r, g, b, 255);
    }

    public void MoveAgent(float[] act)
    {
        m_Eat = false;

        var verDir = Vector2.zero;
        var horDir = Vector2.zero;
        var rotateDir = Vector3.zero;

            var forwardAxis = (int)act[0];
            var rightAxis = (int)act[1];
            var rotateAxis = (int)act[2];
            var eatAxis = (int)act[3];

            switch (forwardAxis)
            {
                case 1:
                    
                    verDir = transform.up;
                break;
                case 2:
                    verDir = -transform.up;
                break;
            }

            switch (rightAxis)
            {
                case 1:
                    horDir = transform.right;
                    break;
                case 2:
                    horDir = -transform.right;
                    break;
            }
            //-------------------------------
            switch (rotateAxis)
            {
                case 1:
                rotateDir = transform.forward;
                break;
                case 2:
                rotateDir = -transform.forward;
                break;
            }
        
        m_Eat = eatAxis==1 ? true : false;
            m_AgentRb.AddForce(verDir * moveSpeed);
            m_AgentRb.AddForce(horDir * moveSpeed);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);

        if (m_AgentRb.velocity.sqrMagnitude > 25f) 
        {
            m_AgentRb.velocity *= 0.95f;
        }


        if (m_Eat)
        {

            var myTransform = transform;
            var rayDir = 2.0f * myTransform.forward;
            Debug.DrawRay(myTransform.position, rayDir, Color.yellow, 0.5f, true);
            
        }
    }

    int stepno = 0;
    public override void AgentAction(float[] vectorAction)
    {
        stepno++;
        if (stepno >= 7500) { stepno = 0; }
        Debug.Log("agent step no :"+stepno);
        MoveAgent(vectorAction);
    }
    
    public override float[] Heuristic()
    {
        Debug.Log("Heuristic");
        var action = new float[4];
        if (Input.GetKey(KeyCode.D))
        {
            action[1] = 1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            action[0] = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            action[1] = 2f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            action[0] = 2f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            action[2] = 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            action[2] = 2f;
        }
        action[3] = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;
        return action;
    }

    public override void AgentReset()
    {
        Debug.Log("agent reseted");
        GameObject.Find("CellsAreaSettings").GetComponent<CellsAreaSettings>().ResetRequest();
        m_Eat = false;
        m_AgentRb.velocity = Vector3.zero;
        transform.position = new Vector3(Random.Range(-m_MyArea.range, m_MyArea.range),
            Random.Range(-m_MyArea.range, m_MyArea.range),0 )
            + area.transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0f,0f, Random.Range(0, 360)));

    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        if (!m_Eat) { return; }
        
        if (collision.gameObject.CompareTag("bacteria"))
        {
            
            collision.gameObject.GetComponent<CellLogic>().OnEaten();
            AddReward(1f);
            if (contribute)
            {
                m_CellsAreaSettings.totalScore += 1;
            }
        }
       
        if (collision.gameObject.CompareTag("platelet"))
        {
           
            collision.gameObject.GetComponent<CellLogic>().OnEaten();

            AddReward(-1f);
            if (contribute)
            {
                m_CellsAreaSettings.totalScore -= 3;
            }
        }
    }




}
