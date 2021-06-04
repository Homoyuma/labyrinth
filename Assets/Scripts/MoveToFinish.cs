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
    //private Vector2 offset = new Vector2(0.5f, 0.5f);
    //[SerializeField] public Material win;
    //[SerializeField] public SpriteRenderer floor;
    public override void Initialize()
    {
        agentRigidbody = GetComponent<Rigidbody2D>();
    }
    public override void OnEpisodeBegin()
    {
        //transform.localPosition = new Vector2(Random.Range(0, PlayerPrefs.GetInt("width")), Random.Range(0, PlayerPrefs.GetInt("height"))) + offset;
        transform.localPosition = new Vector2(Globals.startX[int.Parse(transform.parent.name)] + 0.5f,
            Globals.startY[int.Parse(transform.parent.name)] + 0.5f);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        //int moveX = actions.DiscreteActions[0];//0 = Dont Move; 1 = Left; 2 = Right;
        //int moveY = actions.DiscreteActions[1];//0 = Dont Move; 1 = Back; 2 = Forward;
        /*Vector2 addForce = new Vector2(0,0);
        
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
        */
        float moveSpeed = 5f;
        agentRigidbody.velocity = moveSpeed * new Vector2(moveX, moveY);
        //transform.localPosition += new Vector3(moveX, moveY) * moveSpeed * Time.deltaTime;
        //AddReward(-0.001f);
        //AddReward(-1f / MaxStep);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
        /*switch (Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")))
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
        */
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Finish") == true) {
            AddReward(+1.0f);
            //floor.material = win;
            int index = int.Parse(transform.parent.name);
            int count = Globals.cellArray[index].Count;
            GameObject.Destroy(Globals.finishWall[index].gameObject);
            Globals.finishWall[index] = null;
            for (int i = 0; i < count; i++)
            {
                GameObject.Destroy(Globals.cellArray[index][0].gameObject);
                Globals.cellArray[index].RemoveAt(0);
            }
                Globals.spawner[index].Start();
            EndEpisode();
        }      
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Wall") == true)
        {
            AddReward(-0.0001f);
        }
    }
}
