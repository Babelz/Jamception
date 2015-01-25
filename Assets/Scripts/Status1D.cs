using UnityEngine;
using System.Collections;

public class Status1D : MonoBehaviour {

    private float scoreMax = 100f;
    private float score = 0f;
    private float maxScale = 1f;
    private StateManager stateManager;

	// Use this for initialization
	void Start () {
        stateManager = GameObject.FindGameObjectWithTag("StateManager").GetComponent<StateManager>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = new Vector3(maxScale * (score / scoreMax), transform.localScale.y, transform.localScale.z);
	}

    public void addScore(float amount)
    {
        if (stateManager.State == (int)GameStates.Normal)
        { 
            score += amount;

            if (score > scoreMax)
            {
                GameObject.FindGameObjectWithTag("GameIsReady").GetComponent<GameIsReady1D>().WhatDoWeDoNow();
            }
        }
    }
}
