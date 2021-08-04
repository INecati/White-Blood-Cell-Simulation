using UnityEngine;
using MLAgents;

public class WhiteBloodAgentv2 : Agent
{
    CellsAreaSettings m_CellsAreaSettings;
    public GameObject area;
    CellsArea m_MyArea;
    //bool m_Frozen;
    //bool m_Poisoned;
    //bool m_Satiated;
    //bool m_Shoot;
    bool m_Eat;
    //float m_FrozenTime;
    //float m_EffectTime;
    Rigidbody2D m_AgentRb;
    //float m_LaserLength;
    // Speed of agent rotation.
    public float turnSpeed = 300;

    // Speed of agent movement.
    public float moveSpeed = 2;
    public Material normalMaterial;
    public Material badMaterial;
    public Material goodMaterial;
    public Material frozenMaterial;
    //public GameObject myLaser;
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
        SetResetParameters();
    }

    public override void CollectObservations()
    {
        if (useVectorObs)
        {
            //var localVelocity = transform.InverseTransformDirection(m_AgentRb.velocity);
            //AddVectorObs(localVelocity.x);
            //AddVectorObs(localVelocity.z);
            //AddVectorObs(System.Convert.ToInt32(m_Frozen));
            //AddVectorObs(System.Convert.ToInt32(m_Shoot));
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
        /*
        if (Time.time > m_FrozenTime + 4f && m_Frozen)
        {
            Unfreeze();
        }
        if (Time.time > m_EffectTime + 0.5f)
        {
            if (m_Poisoned)
            {
                Unpoison();
            }
            if (m_Satiated)
            {
                Unsatiate();
            }
        }
        */
        var verDir = Vector2.zero;
        var horDir = Vector2.zero;
        var rotateDir = Vector3.zero;

        //if (!m_Frozen)
        //{
        var shootCommand = false;
        var forwardAxis = (int)act[0];
        var rightAxis = (int)act[1];
        var rotateAxis = (int)act[2];
        var eatAxis = (int)act[3];
        /*
        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward;
                break;
            case 2:
                dirToGo = -transform.forward;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right;
                break;
            case 2:
                dirToGo = -transform.right;
                break;
        }
        */
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
                //rotateDir = -transform.up;
                rotateDir = transform.forward;
                //rotateDir = new Vector3(0, 0, 1);
                //rotateDir = new Vector3(0,0,50);
                break;
            case 2:
                rotateDir = -transform.forward;
                break;
        }

        m_Eat = eatAxis == 1 ? true : false;
        /*
        switch (shootAxis)
        {
            case 1:
                eatCommand = true;
                break;
        }
        if (shootCommand)
        {
            m_Shoot = true;
            dirToGo *= 0.0f;//was 0.5
            m_AgentRb.velocity *= 0.75f;
        */
        //Debug.Log("MoveAgent");
        m_AgentRb.AddForce(verDir * moveSpeed);
        m_AgentRb.AddForce(horDir * moveSpeed);
        //m_AgentRb.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);
        //transform.eulerAngles = Vector3.forward*50;
        //}

        if (m_AgentRb.velocity.sqrMagnitude > 25f) // slow it down
        {
            m_AgentRb.velocity *= 0.95f;
        }


        if (m_Eat)
        {

            var myTransform = transform;
            //myLaser.transform.localScale = new Vector3(1f, 1f, m_LaserLength);
            //var rayDir = 25.0f * myTransform.forward; //range
            var rayDir = 2.0f * myTransform.forward;
            //Debug.DrawRay(myTransform.position, rayDir, Color.red, 0f, true);
            Debug.DrawRay(myTransform.position, rayDir, Color.yellow, 0.5f, true);
            RaycastHit hit;
            //if (Physics.SphereCast(transform.position, 2f, rayDir, out hit, 25f))

            /*
            if (Physics.SphereCast(myTransform.position, 1f, rayDir, out hit, 2f))
            {
                
                if (hit.collider.gameObject.CompareTag("agent"))
                {
                    //hit.collider.gameObject.GetComponent<FoodCollectorAgent>().Freeze();
                    hit.collider.gameObject.GetComponent<WhiteBloodAgentv1>().Freeze();
                    AddReward(-2);//new

                }
                if (hit.collider.gameObject.CompareTag("food")) {
                    
                    Satiate();
                    hit.collider.gameObject.GetComponent<FoodLogic>().OnEaten();
                    AddReward(1);
                    if (contribute)
                    {
                        m_FoodCollecterSettings.totalScore += 1;
                    }
                    //Debug.Log("food shot with laser");
                }
                if (hit.collider.gameObject.CompareTag("badFood"))
                {
                    Poison();
                    hit.collider.gameObject.GetComponent<FoodLogic>().OnEaten();
                    AddReward(-1);
                    if (contribute)
                    {
                        m_FoodCollecterSettings.totalScore -= 1;
                    }
                    //Debug.Log("badfood shot with laser");
                }
                
                
                
            }*/
        }
        else
        {
            //myLaser.transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }

    /*
    void Freeze()
    {
        gameObject.tag = "frozenAgent";
        m_Frozen = true;
        m_FrozenTime = Time.time;
        gameObject.GetComponentInChildren<Renderer>().material = frozenMaterial;
    }

    void Unfreeze()
    {
        m_Frozen = false;
        gameObject.tag = "agent";
        gameObject.GetComponentInChildren<Renderer>().material = normalMaterial;
    }

    void Poison()
    {
        m_Poisoned = true;
        m_EffectTime = Time.time;
        gameObject.GetComponentInChildren<Renderer>().material = badMaterial;
    }

    void Unpoison()
    {
        m_Poisoned = false;
        gameObject.GetComponentInChildren<Renderer>().material = normalMaterial;
    }

    void Satiate()
    {
        m_Satiated = true;
        m_EffectTime = Time.time;
        gameObject.GetComponentInChildren<Renderer>().material = goodMaterial;
    }

    void Unsatiate()
    {
        m_Satiated = false;
        gameObject.GetComponentInChildren<Renderer>().material = normalMaterial;
    }
    */
    int stepno = 0;
    public override void AgentAction(float[] vectorAction)
    {
        stepno++;
        //Debug.Log("agent step no :"+stepno);
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
        GameObject.Find("CellsAreaSettings").GetComponent<CellsAreaSettings>().ResetRequest();//new

        //Unfreeze();
        //Unpoison();
        //Unsatiate();
        m_Eat = false;
        m_AgentRb.velocity = Vector3.zero;
        //myLaser.transform.localScale = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(Random.Range(-m_MyArea.range, m_MyArea.range),
            Random.Range(-m_MyArea.range, m_MyArea.range), 0)
            + area.transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(0, 360)));

        SetResetParameters();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!m_Eat) { return; }
        //Debug.Log("crash with" + collision.transform.name);
        if (collision.gameObject.CompareTag("bacteria"))
        {
            //Satiate();
            collision.gameObject.GetComponent<CellLogic>().OnEaten();
            AddReward(1f);
            if (contribute)
            {
                m_CellsAreaSettings.totalScore += 1;
            }
        }
        if (collision.gameObject.CompareTag("redBloodCell"))
        {
            //Poison();
            collision.gameObject.GetComponent<CellLogic>().OnEaten();

            AddReward(-1f);
            if (contribute)
            {
                m_CellsAreaSettings.totalScore -= 1;
            }
        }
        if (collision.gameObject.CompareTag("platelet"))
        {
            //Poison();
            collision.gameObject.GetComponent<CellLogic>().OnEaten();

            AddReward(-1f);
            if (contribute)
            {
                m_CellsAreaSettings.totalScore -= 1;
            }
        }
    }



    public void SetLaserLengths()
    {
        //m_LaserLength = Academy.Instance.FloatProperties.GetPropertyWithDefault("laser_length", 1.0f);
        //m_LaserLength = Academy.Instance.FloatProperties.GetPropertyWithDefault("laser_length", 0.2f);
    }

    public void SetAgentScale()
    {
        float agentScale = Academy.Instance.FloatProperties.GetPropertyWithDefault("agent_scale", 1.0f);
        gameObject.transform.localScale = new Vector3(agentScale, agentScale, agentScale);
    }

    public void SetResetParameters()
    {
        SetLaserLengths();
        SetAgentScale();
    }
}
