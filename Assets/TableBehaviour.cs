using UnityEngine;
using System.Collections.Generic;

public class TableBehaviour : MonoBehaviour {


    public int InitialPizzaCount = 3;

    private List<GameObject> pizzas =  new List<GameObject>();

    public GameObject Pizza;

    public bool HasPizza
    {
        get
        {
            return pizzas.Count > 0;
        }
    }
	// Use this for initialization
	void Start () {
        
        for (int i = 0; i < InitialPizzaCount; i++)
        {
            GameObject pizza = GameObject.Instantiate(Pizza) as GameObject;
            Vector3 pos = this.transform.position;
            pos.z -= 3f;
            pos.x += i * 0.45f;
            pizza.transform.position = pos;
            pizzas.Add(pizza);
            
        }
	}
	
    public void GimmePizza()
    {
        if (pizzas.Count == 0) return;

        GameObject pizza = pizzas[pizzas.Count - 1];
        pizzas.Remove(pizza);
        Destroy(pizza);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
