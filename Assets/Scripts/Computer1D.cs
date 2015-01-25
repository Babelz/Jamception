using UnityEngine;
using System.Collections;

public class Computer1D : MonoBehaviour {
    public float minTime = 0.5f;
    public float maxTime = 5f;
    public float scoreAmount = 0.65f;

    private float timer;
    private float scoreTimer;
    private int state;
    private enum ComputerStates { Normal, Bugged, BSOD };

    private KeyCode fixKey;
    private KeyCode[] possibleKeys = { KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V };

    private StateManager stateManager;

	// Use this for initialization
	void Start () {
        stateManager = GameObject.FindGameObjectWithTag("StateManager").GetComponent<StateManager>();
        state = (int)ComputerStates.Normal;
        timer = Random.Range(minTime, maxTime);
        scoreTimer = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (stateManager.State == (int)GameStates.Normal)
        {
            timer -= Time.deltaTime;

            if (state == (int)ComputerStates.Normal)
            {
                scoreTimer -= Time.deltaTime;

                if (scoreTimer < 0)
                {
                    GameObject statusBar = GameObject.FindGameObjectWithTag("StatusBar");
                    statusBar.GetComponent<Status1D>().addScore(scoreAmount);
                    scoreTimer = 1;
                }
                else if (timer < 0)
                {
                    gameObject.renderer.material.color = Color.green;
                    state = (int)ComputerStates.Bugged;
                    timer = 5;
                    fixKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
                }
            }

            if (timer < 0 && state == (int)ComputerStates.Bugged)
            {
                gameObject.renderer.material.color = Color.blue;
                state = (int)ComputerStates.BSOD;
                timer = 10;
            }
            else if (timer < 0 && state == (int)ComputerStates.BSOD)
            {
                gameObject.renderer.material.color = Color.white;
                state = (int)ComputerStates.Normal;
                timer = Random.Range(minTime, maxTime);
                scoreTimer = 1;
            }
        }
	}

    public bool IsBugged()
    {
        return (state == (int)ComputerStates.Bugged);
    }

    public void Fix()
    {
        gameObject.renderer.material.color = Color.white;
        state = (int)ComputerStates.Normal;
        timer = Random.Range(minTime, maxTime);
        scoreTimer = 1;
    }

    public KeyCode GetFixKey()
    {
        return fixKey;
    }

    public void AWildBSODAppears()
    {
        gameObject.renderer.material.color = Color.blue;
        state = (int)ComputerStates.BSOD;
        timer = 10;
    }
}
