using UnityEngine;
using System.Collections;

public class Computer1D : MonoBehaviour {
    public float minTime = 0.5f;
    public float maxTime = 5f;
    public float scoreAmount = 1f;

    private float timer;
    private float scoreTimer;
    private int state;
    private enum States { Normal, Bugged, BSOD };

	// Use this for initialization
	void Start () {
        state = (int)States.Normal;
        timer = Random.Range(minTime, maxTime);
        scoreTimer = 1;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;

        if (state == (int)States.Normal)
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
                state = (int)States.Bugged;
                timer = 3;
            }
        }
        
        if (timer < 0 && state == (int)States.Bugged)
        {
            gameObject.renderer.material.color = Color.blue;
            state = (int)States.BSOD;
            timer = 10;
        }
        else if (timer < 0 && state == (int)States.BSOD)
        {
            gameObject.renderer.material.color = Color.white;
            state = (int)States.Normal;
            timer = Random.Range(minTime, maxTime);
            scoreTimer = 1;
        }
	}

    public bool IsBugged()
    {
        return (state == (int)States.Bugged);
    }

    public void Fix()
    {
        gameObject.renderer.material.color = Color.white;
        state = (int)States.Normal;
        timer = Random.Range(minTime, maxTime);
        scoreTimer = 1;
    }
}
