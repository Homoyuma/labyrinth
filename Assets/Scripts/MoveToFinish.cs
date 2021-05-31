using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToFinish : Agent
{
    public Rigidbody2D agentRigidbody;
    [SerializeField] private Transform targetTransform;
    private Vector2 offset = new Vector2(0.5f, 0.5f);
    //[SerializeField] public Material win;
    //[SerializeField] public SpriteRenderer floor;
    private void Awake()
    {
        agentRigidbody = GetComponent<Rigidbody2D>();
    }
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector2(Random.Range(0, PlayerPrefs.GetInt("width")), Random.Range(0, PlayerPrefs.GetInt("height"))) + offset;
        //transform.localPosition = new Vector2(0.5f, 0.5f);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        //float moveX = actions.ContinuousActions[0];
        //float moveY = actions.ContinuousActions[1];
        int moveX = actions.DiscreteActions[0];//0 = Dont Move; 1 = Left; 2 = Right;
        int moveY = actions.DiscreteActions[1];//0 = Dont Move; 1 = Back; 2 = Forward;
        Vector2 addForce = new Vector2(0,0);
        
        switch (moveX)
        {
            case 0: addForce.x = 0f;break;
            case 1: addForce.x = -1f; break;
            case 2: addForce.x = +1f; break;
        }
        switch (moveY)
        {
            case 0: addForce.y = 0f; break;
            case 1: addForce.y = -1f; break;
            case 2: addForce.y = +1f; break;
        }
        float moveSpeed = 3f;
        agentRigidbody.velocity = addForce * moveSpeed + new Vector2(0, 0);
        //transform.localPosition = new Vector2(moveX, moveY) + moveSpeed * addForce;
        //AddReward(-1f / MaxStep);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        //continuousActions[0] = Input.GetAxisRaw("Horizontal");
        //continuousActions[1] = Input.GetAxisRaw("Vertical");
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        switch (Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")))
        {
            case -1: discreteActions[0] = 1;break;
            case 0: discreteActions[0] = 0; break;
            case +1: discreteActions[0] = 2; break;
        }
        switch (Mathf.RoundToInt(Input.GetAxisRaw("Vertical")))
        {
            case -1: discreteActions[1] = 1; break;
            case 0: discreteActions[1] = 0; break;
            case +1: discreteActions[1] = 2; break;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.StartsWith("FinishWall")) {
            AddReward(+1f);
            //floor.material = win;
            if (transform.name == "1")
            {
                int count = Globals.cellArray1.Count;
                GameObject.Destroy(Globals.finishWall1.gameObject);
                Globals.finishWall1 = null;
                for (int i = 0; i < count; i++)
                {
                    GameObject.Destroy(Globals.cellArray1[0].gameObject);
                    Globals.cellArray1.RemoveAt(0);
                }
                Globals.spawner1.Start();
                EndEpisode();
            }
            if (transform.name == "2")
            {
                int count = Globals.cellArray2.Count;
                GameObject.Destroy(Globals.finishWall2.gameObject);
                Globals.finishWall2 = null;
                for (int i = 0; i < count; i++)
                {
                    GameObject.Destroy(Globals.cellArray2[0].gameObject);
                    Globals.cellArray2.RemoveAt(0);
                }
                Globals.spawner2.Start();
                EndEpisode();
            }
            if (transform.name == "3")
            {
                int count = Globals.cellArray3.Count;
                GameObject.Destroy(Globals.finishWall3.gameObject);
                Globals.finishWall3 = null;
                for (int i = 0; i < count; i++)
                {
                    GameObject.Destroy(Globals.cellArray3[0].gameObject);
                    Globals.cellArray3.RemoveAt(0);
                }
                Globals.spawner3.Start();
                EndEpisode();
            }
            if (transform.name == "4")
            {
                int count = Globals.cellArray4.Count;
                GameObject.Destroy(Globals.finishWall4.gameObject);
                Globals.finishWall4 = null;
                for (int i = 0; i < count; i++)
                {
                    GameObject.Destroy(Globals.cellArray4[0].gameObject);
                    Globals.cellArray4.RemoveAt(0);
                }
                Globals.spawner4.Start();
                EndEpisode();
            }

        }
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name.StartsWith("Wall"))
        {
            AddReward(-0.0001f);
        }
    }
}
