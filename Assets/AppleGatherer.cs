using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using MLAgents;
using TMPro;

public class AppleGatherer : Agent
{
    //public GameObject area;
    //PyramidArea m_MyArea;
    Rigidbody m_AgentRb;
    //PyramidSwitch m_SwitchLogic;
    //public GameObject areaSwitch;
    private AppleFarm appleFarm;
    public bool useVectorObs;
    //-------------
    //public LayerMask treeLayer;
    public float rotateSpeed;
    public int appleNumber=0;
    public GameObject[] apples;
    public TextMeshProUGUI SpeedText;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public bool hudInfo;
    public int totalCollected = 0;
    private void Start()
    {
        apples = new GameObject[5];
    }
    private void Update()
    {
        if (!hudInfo) { return; }


        text2.text = "Apples: " + appleNumber;
        text1.text = "Apples Delivered: " + totalCollected;
        SpeedText.text = "Speed: " + Mathf.FloorToInt(Vector3.Distance(Vector3.zero, m_AgentRb.velocity));
        RaycastHit hit;
        //text1.text = "Apples: " + 0;
        if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, 20f) && hit.transform.CompareTag("tree")) {

            //text1.text = "apples" + hit.transform.gameObject.GetComponent<AppleTree>().appleNumber;
        }

        //_--------
    }
    public override void InitializeAgent()
    {
        base.InitializeAgent();
        m_AgentRb = GetComponent<Rigidbody>();
        //m_MyArea = area.GetComponent<PyramidArea>();
        appleFarm = GetComponentInParent<AppleFarm>();
        //m_SwitchLogic = areaSwitch.GetComponent<PyramidSwitch>();
    }

    public override void CollectObservations()
    {
        if (useVectorObs)
        {
            //Debug.Log("it works");
            //Debug.Log("forward:" + transform.forward.ToString());
            //Debug.Log("after invert:" + transform.InverseTransformDirection(transform.forward).ToString());
            //Debug.Log("velocity" + m_AgentRb.velocity.ToString());
            //Debug.Log(transform.InverseTransformDirection(m_AgentRb.velocity).ToString());
            //AddVectorObs(m_SwitchLogic.GetState());
            AddVectorObs(appleNumber);
            RaycastHit hit;
            int applesOnTree = 0;
            if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, 20f) && hit.transform.CompareTag("tree"))
            {

                applesOnTree=hit.transform.gameObject.GetComponent<AppleTree>().appleNumber;
            }
            AddVectorObs(applesOnTree);
            AddVectorObs(transform.InverseTransformDirection(m_AgentRb.velocity));

        }
    }

    public void MoveAgent(float[] act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var move = Mathf.FloorToInt(act[0]);
        switch (move)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
        }
        var rotate = Mathf.FloorToInt(act[1]);
        switch (rotate)
        {
            case 1:
                rotateDir = transform.up * 1f;
                break;
            case 2:
                rotateDir = transform.up * -1f;
                break;
        }
        transform.Rotate(rotateDir, Time.deltaTime * rotateSpeed);
        m_AgentRb.AddForce(dirToGo * 2f, ForceMode.VelocityChange);
        //m_AgentRb.AddForce(dirToGo * 2f, ForceMode.VelocityChange);
    }

    public override void AgentAction(float[] vectorAction)
    {
        //AddReward(-1f / maxStep);
        MoveAgent(vectorAction);
    }

    public override float[] Heuristic()
    {
        //Debug.Log("Heuristic");
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
        //action[3] = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;
        return action;
        ///--------
        if (Input.GetKey(KeyCode.D))
        {
            return new float[] { 3 };
        }
        if (Input.GetKey(KeyCode.W))
        {
            return new float[] { 1 };
        }
        if (Input.GetKey(KeyCode.A))
        {
            return new float[] { 4 };
        }
        if (Input.GetKey(KeyCode.S))
        {
            return new float[] { 2 };
        }
        return new float[] { 0 };
    }
    int episodeNumber = 1;
    public override void AgentReset()
    {
        Debug.Log("Episode: " + episodeNumber);
        episodeNumber++;
        //Debug.Log("Agent Reseted");
        //var enumerable = Enumerable.Range(0, 9).OrderBy(x => Guid.NewGuid()).Take(9);
        //var items = enumerable.ToArray();

        //Debug.Log(gameObject.name);//sil
        //m_MyArea.CleanPyramidArea();

        m_AgentRb.velocity = Vector3.zero;
        //m_MyArea.PlaceObject(gameObject, items[0]);
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
        transform.localPosition = new Vector3(0f, 0.56f, 0f);
        transform.parent.GetComponent<AppleFarm>().ResetArea();
        totalCollected = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("tree")) {
            //Debug.Log("speed: " + Vector3.Distance(Vector3.zero, m_AgentRb.velocity));
            //Debug.Log("speed: "+collision.relativeVelocity.magnitude);
            int droppedApples;
            droppedApples= collision.gameObject.GetComponent<AppleTree>().dealDamage(collision.relativeVelocity.magnitude);
            //reward
            AddReward(0.2f * droppedApples);
        }
        if (collision.gameObject.CompareTag("apple"))
        {
            if (appleNumber>=5) { return; }
            AddReward(0.2f);

            //Debug.Log("apple collected");
            GameObject apple;
            apple = collision.gameObject;
            //apple.GetComponent<Rigidbody>().useGravity = false;
            apple.GetComponent<SphereCollider>().enabled = false;
            Destroy(apple.GetComponent<Rigidbody>());
            apple.transform.SetParent(transform);
            //apple.transform.position = transform.position + new Vector3(0, 2*appleNumber, 0);
            
            apples[appleNumber] = apple;


            appleNumber++;
            apple.transform.localPosition = Vector3.up * appleNumber*0.5f ;
            //SetReward(2f);
            //Done();
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("goal")) {
            //Debug.Log("unload area");

            for (int i=0;i<appleNumber;i++) {
                Destroy(apples[i]);
                AddReward(2);
                totalCollected++;
            }
            appleNumber = 0;
        }
    }

}
