using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    public float Speed = 5f;

    public float Horizontal
    {
        get;
        private set;
    }

    public float Vertical
    {
        get;
        private set;
    }

	// Use this for initialization
	void Start () {
        Physics2D.gravity = new Vector3(0f, 0f, 0f);
        Physics2D.IgnoreCollision(transform.GetComponent<BoxCollider2D>(), transform.GetComponent<CircleCollider2D>());
	}
	
	// Update is called once per frame
	void Update () {     
	}

    void FixedUpdate()
    {
        Horizontal = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
        Vertical = Input.GetKey(KeyCode.S) ? -1f : Input.GetKey(KeyCode.W) ? 1f : 0f;

        var rot = Quaternion.identity;
        if (Horizontal < 0f)
        {
            rot.eulerAngles = new Vector3(0, 0, -90f);
            transform.rotation = rot;
        }
        else if (Horizontal > 0f)
        {
            rot.eulerAngles = new Vector3(0, 0, 90f);
            transform.rotation = rot;
        }

        if (Vertical < 0f)
        {
            rot.eulerAngles = new Vector3(0, 0, 0f);
            transform.rotation = rot;
        }
        else if (Vertical > 0f)
        {
            rot.eulerAngles = new Vector3(0, 0, 180f);
            transform.rotation = rot;
        }
        //Debug.Log(Vertical);
        transform.position += new Vector3(1f, 0f, 0f)  *  Horizontal * Speed * Time.deltaTime;
        transform.position += new Vector3(0f, 1f, 0f) * Vertical * Speed * Time.deltaTime;

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.name == "LaptopTrigger")
        {
            if (!Input.GetKeyDown(KeyCode.E)) return;
            SmashLaptop(col.gameObject);
        }
    }

    void SmashLaptop(GameObject laptopTrigger)
    {
        // head level obj
        var obj = laptopTrigger.transform.parent;
        var rage = obj.GetComponent<LaptopRage>();
        rage.OnFlippingFinished += rage_OnFlippingFinished;
        rage.InitiateRage();
    }

    void rage_OnFlippingFinished(LaptopRage sender)
    {
        sender.OnFlippingFinished -= rage_OnFlippingFinished;
    }
}
