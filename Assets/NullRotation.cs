using UnityEngine;
using System.Collections;

public class NullRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
    private Quaternion rotation;
    void Awake()
    {
        rotation = transform.rotation;
    }

	// Update is called once per frame
	void Update () {
	    
	}

    void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
