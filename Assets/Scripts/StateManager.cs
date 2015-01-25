using UnityEngine;
using System.Collections;

public enum GameStates { Normal, EnteringTheMatrix, GameIsReady };

public class StateManager : MonoBehaviour {

    private int state;

    public int State
    {
        get { return state; }
        set { state = value; }
    }

	// Use this for initialization
	void Start () {
        state = (int)GameStates.Normal;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
