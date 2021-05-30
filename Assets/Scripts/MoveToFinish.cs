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
    //[SerializeField] public Material win;
    //[SerializeField] public SpriteRenderer floor;
    private void Awake()
    {
        agentRigidbody = GetComponent<Rigidbody2D>();
    }
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector2(Random.Range(0, 3), Random.Range(0, 3));
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
            int count = Globals.cellArray.Count;
            GameObject.Destroy(Globals.finishWall.gameObject);
            Globals.finishWall = null;
            for (int i = 0; i < count; i++)
            {
                GameObject.Destroy(Globals.cellArray[0].gameObject);
                Globals.cellArray.RemoveAt(0);
            }
            Globals.spawner.Start();
            EndEpisode();
        }
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name.StartsWith("Wall"))
        {
            AddReward(-0.0001f);
        }
    }
}