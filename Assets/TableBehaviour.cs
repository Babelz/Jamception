using UnityEngine;
using System.Collections.Generic;

public class TableBehaviour : MonoBehaviour {
    private static System.Random random = new System.Random();

    public bool SpawnPizza = false;
    public GameObject PizzaBox;

    // pizza box 
    private GameObject box;

    public bool HasPizza
    {
        get { return box != null; }
    }

    public PizzaSpawner PizzaBehaviour
    {
        get { return box.GetComponent<PizzaSpawner>();  }
    }

	// Use this for initialization
	void Start () {

        if (!SpawnPizza) return;

        var container = transform.GetChild(0);
        box = Instantiate(PizzaBox) as GameObject;
        box.transform.parent = container;
        box.transform.position = container.transform.position;

        var spawner = box.AddComponent<PizzaSpawner>();
        spawner.Spawn(box, "Pizza", random.Next(1,3));

	}

	// Update is called once per frame
	void Update () {
	
	}
}
