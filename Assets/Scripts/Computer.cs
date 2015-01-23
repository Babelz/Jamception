using UnityEngine;
using System.Collections;

public class Computer : MonoBehaviour {
    public float minTime = 0.5f;
    public float maxTime = 5f;

    private float timer;
    private int state;
    private enum States { Normal, Bugged, BSOD };

	// Use this for initialization
	void Start () {
        state = (int)States.Normal;
        timer = Random.Range(minTime, maxTime);
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;

        if (timer < 0 && state == (int)States.Normal)
        {
            gameObject.renderer.material.color = Color.green;
            state = (int)States.Bugged;
            timer = 3;
        }
        else if (timer < 0 && state == (int)States.Bugged)
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
    }
}
