using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {

    private enum States { Normal, EnteringTheMatrix };
    private int state;

	// Use this for initialization
	void Start () {
	    state = (int)States.Normal;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool IsEntering()
    {
        return (state == (int)States.EnteringTheMatrix);
    }

    public void EnterTheMatrix()
    {
        state = (int)States.EnteringTheMatrix;
    }
}
