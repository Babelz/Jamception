using UnityEngine;
using System.Collections;
using System;

public class ArmController : MonoBehaviour
{
    #region Vars
    public Action<Collision> callback;
    #endregion

    // Use this for initialization
    private void Start()
    {
	
	}
	
	// Update is called once per frame
    private void Update()
    {
	
	}

    private void OnCollisionEnter(Collision col)
    {
        if (callback != null)
        {
            callback(col);
        }

        Debug.Log(col.gameObject.name);
        Debug.Log("HAISTA VITTU T GIGGI HIIRI");
    }
}
