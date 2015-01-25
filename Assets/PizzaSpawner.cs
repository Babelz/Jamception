using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PizzaSpawner : MonoBehaviour {

    private System.Random r = new System.Random();

	// Use this for initialization
	void Start () {

	}

    public void Spawn(GameObject box, string prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject pizza = GameObject.Instantiate(Resources.Load(prefab)) as GameObject;
            pizza.transform.parent = box.transform;
            Vector3 pos = box.transform.position;

            pos.y -= 0.25f;
            pos.x -= 0.5f;
            pos.x += i * 0.45f;
            pos.z -= 0.5f;
            pizza.transform.position = pos;
            
        }
    }

    public bool HasPizza
    {
        get
        {
            return transform.childCount > 1;
        }
    }

    public void GimmePizza()
    {
        
        if (!HasPizza) return;

        var pizza = transform.GetChild(transform.childCount - 1);
        
        Destroy(pizza.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
