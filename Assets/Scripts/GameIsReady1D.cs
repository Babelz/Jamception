using UnityEngine;
using System.Collections;

public class GameIsReady1D : MonoBehaviour {

    private float timer;
    private StateManager stateManager;
	// Use this for initialization
	void Start () {
        stateManager = GameObject.FindGameObjectWithTag("StateManager").GetComponent<StateManager>();
        GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (stateManager.State == (int)GameStates.GameIsReady)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera1D>().EnterTheMatrix();
            }
        }
	}

    public void WhatDoWeDoNow()
    {
        stateManager.State = (int)GameStates.GameIsReady;
        timer = 2f;
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
