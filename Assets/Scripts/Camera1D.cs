using UnityEngine;
using System.Collections;

public class Camera1D : MonoBehaviour {

    private GameObject[] computers;
    private Transform start;
    private Transform end;
    private float speed = 0.5f;
    private float startTime;
    private float journeyLength;
    private StateManager stateManager;
    private Vector3 targetOffset = new Vector3(-0.05f, 0.3f, 0f);

	// Use this for initialization
	void Start () {
        computers = GameObject.FindGameObjectsWithTag("Computer");
        stateManager = GameObject.FindGameObjectWithTag("StateManager").GetComponent<StateManager>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (stateManager.IsEntering())
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(start.position, (end.position + targetOffset), fracJourney);

            if (transform.position == end.position)
            {
                Application.LoadLevel(3);
            }
        }
	}

    public void EnterTheMatrix()
    {

        foreach(GameObject computer in computers)
        {
            computer.transform.renderer.material.color = Color.black;
        }

        stateManager.EnterTheMatrix();
        int random = Random.Range(0, computers.Length);

        start = transform;
        end = computers[random].transform;

        startTime = Time.time;
        journeyLength = Vector3.Distance(start.position, end.position);
    }
}
