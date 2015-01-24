using UnityEngine;
using System.Collections;

public class Sight : MonoBehaviour {

    public float FOV = 110f;

    private GameObject player;
    private CircleCollider2D col;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        col = GetComponent<CircleCollider2D>();
        // skip own collision
        Physics2D.IgnoreCollision(transform.collider2D, col);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 d = transform.position + transform.up * col.radius ;
        Vector3 a = Quaternion.AngleAxis(-FOV/2f, Vector3.up) * d;
        Vector3 b = -a;
        
        Debug.DrawLine(transform.position, a);
        Debug.DrawLine(transform.position, b);
	}

    void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.gameObject != player) return;

       
        // get the angle
        Vector3 direction = other.transform.position - transform.position; 
        float angle = Vector3.Angle(direction, transform.up);
        //Debug.Log(angle);
        // angle needs to be less than half of the fov
        if (angle > FOV * 0.5f) return;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up, direction.normalized, col.radius);
        // no hit
        if (hit.collider == null) return;
        if (hit.collider.gameObject == player)
        {
            Debug.Log("Player in sight");
        }
        
    }


}
