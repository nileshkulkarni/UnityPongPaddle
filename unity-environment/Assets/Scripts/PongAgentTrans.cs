﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PongAgentTrans : Agent
{

    [Header("Specific to Pong")]
    public GameObject myPong;
	public Vector3 initial_position;
	public float initial_x =0.0f;
	public override void InitializeAgent()
    {
		initial_position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
		initial_x = 0.0f;
    }
    public override List<float> CollectState()
    {
        List<float> state = new List<float>();
        state.Add(gameObject.transform.position.x / 5f);
        state.Add(gameObject.GetComponent<Rigidbody>().velocity.x / 5f);
        // state.Add(gameObject.transform.position.z);
        state.Add(gameObject.transform.rotation.x);
        state.Add(gameObject.transform.rotation.z);
        state.Add((myPong.transform.position.x - gameObject.transform.position.x) / 5f);
        state.Add((myPong.transform.position.y - gameObject.transform.position.y) / 5f);
        state.Add((myPong.transform.position.z - gameObject.transform.position.z) / 5f);
        state.Add(myPong.transform.GetComponent<Rigidbody>().velocity.x / 5f);
        state.Add(myPong.transform.GetComponent<Rigidbody>().velocity.y / 5f);
        state.Add(myPong.transform.GetComponent<Rigidbody>().velocity.z / 5f);

        return state;
    }

    public override void AgentStep(float[] act)
    {
		reward = 0.0f;
		if (brain.brainParameters.actionSpaceType == StateType.continuous)
        {
            float xrotate = act[0];
            float zrotate = act[1];
            // float zmove = act[2];
            float xmove = act[2];

            if (Math.Abs(act[2]) <= 0.25)
            {
                xmove = act[2];
            }
            else
            {
                xmove = 0.0f;
            }

			/*
            if (Math.Abs(gameObject.transform.position.x) > 1.5f)
            {
                xmove = 0f;
            }
			*/
			float relative_position_x = gameObject.transform.position.x - initial_x;
			if ((relative_position_x < 1.5f && xmove > 0f) ||
				(relative_position_x > -1.5f && xmove < 0f))
			{

				if ((xmove + relative_position_x) > 1.5f) {
					xmove = 1.5f - relative_position_x;
				} else if ((xmove + relative_position_x) < -1.5f) {
					xmove = -1.5f - relative_position_x;
				}
				gameObject.transform.Translate(new Vector3(xmove, 0, 0));
				//reward = 0.0001f;
			}
            //gameObject.transform.position = new Vector3(xmove, gameObject.transform.position.y, gameObject.transform.position.z);

            if ((gameObject.transform.rotation.x < 0.25f && xrotate > 0f) ||
                (gameObject.transform.rotation.x > -0.25f && xrotate < 0f))
            {
                //gameObject.transform.Rotate(new Vector3(1, 0, 0), xrotate);
            }
            if ((gameObject.transform.rotation.z < 0.25f && zrotate > 0f) ||
                (gameObject.transform.rotation.z > -0.25f && zrotate < 0f))
            {
                //gameObject.transform.Rotate(new Vector3(0, 0, 1), zrotate);
            }
            if (!done) reward += 0.1f;
        }

//        if ((myPong.transform.position.y - gameObject.transform.position.y) < -0.5f ||
//            Mathf.Abs(myPong.transform.position.x - gameObject.transform.position.x) > 3.0f ||
//            Mathf.Abs(myPong.transform.position.z - gameObject.transform.position.z) > 1f)
//        {
		if ((myPong.transform.position.y - gameObject.transform.position.y) < -0.5f ||
			Mathf.Abs(myPong.transform.position.z - gameObject.transform.position.z) > 1f)
		{
            done = true;
            reward = -1f;
        }
    }

    public override void AgentReset()
    {
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		gameObject.transform.position = new Vector3 (initial_position.x, initial_position.y, initial_position.z);

        gameObject.transform.Rotate(new Vector3(1, 0, 0), UnityEngine.Random.Range(-10f, 10f));
        gameObject.transform.Rotate(new Vector3(0, 0, 1), UnityEngine.Random.Range(-10f, 10f));

        myPong.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        myPong.transform.position = new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), 2f, UnityEngine.Random.Range(-0.25f, 0.25f)) + gameObject.transform.position;
    }

    public override void AgentOnDone()
    {

    }
}

