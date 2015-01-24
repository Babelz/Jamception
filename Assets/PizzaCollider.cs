using UnityEngine;
using System.Collections;

public class PizzaCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag != "Table" || !Input.GetKeyUp(KeyCode.Space)) return;

        var table = other.GetComponent<TableBehaviour>();
        if (!table.HasPizza) return;
        
        table.GimmePizza();
        //transform.parent.GetComponent<PlayerBehaviour>()
    }
}
