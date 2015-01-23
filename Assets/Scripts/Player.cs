using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float speed = 0.1f;
    public GameObject[] computers;

	// Use this for initialization
	void Start () {
        computers = GameObject.FindGameObjectsWithTag("Computer");
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.right * Input.GetAxis("Horizontal") * speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
	        foreach (GameObject computer in computers)
            {
                Vector2 heading = computer.transform.position - transform.position;
                float distance = heading.magnitude;
                Vector2 direction = heading / distance;

                if (heading.sqrMagnitude < 0.25 && computer.GetComponent<Computer>().IsBugged())
                {
                    computer.renderer.material.color = Color.gray;
                    computer.GetComponent<Computer>().Fix();
                }
            }
        }
	}
}
