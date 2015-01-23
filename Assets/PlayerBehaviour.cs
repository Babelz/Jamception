using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    public float Speed = 5f;
	// Use this for initialization
	void Start () {
        Physics2D.gravity = new Vector3(0f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        transform.position += transform.right * Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        transform.position += transform.up * Input.GetAxis("Vertical") * Speed * Time.deltaTime;
    }
}
