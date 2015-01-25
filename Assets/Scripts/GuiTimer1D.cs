using UnityEngine;
using System.Collections;
using System;

public class GuiTimer1D : MonoBehaviour {
    float minutes;
    float hours;

    private StateManager stateManager;
	// Use this for initialization
	void Start () {
        stateManager = GameObject.FindGameObjectWithTag("StateManager").GetComponent<StateManager>();
        hours = 48f;
        minutes = 00f;
        guiText.text = hours.ToString() + minutes.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        if (stateManager.State == (int)GameStates.Normal)
        {
            minutes -= Time.deltaTime * 30;
            if (minutes < 0f)
            {
                minutes += 60f;
                hours -= 1f;

                if (hours < 0f)
                {
                    Application.LoadLevel(Application.loadedLevel);
                }
            }
            guiText.text = hours.ToString("00") + ":" + minutes.ToString("00") + " time left";
        }
        else
            guiText.enabled = false;
	}
}
