using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System;

using Random = System.Random;

public class PlayerController : MonoBehaviour
{
    #region Vars
    private GameObject playerCamera;
    private GameObject foodItem;

    private Random random;

    private string[] foods;
    #endregion

    public void GetFood()
    {
        if (!HasFood())
        {
            ChangeFood(foods[random.Next(0, foods.Length - 1)]);

            foodItem.renderer.enabled = true;
        }
    }
    public bool HasFood()
    {
        return foodItem.renderer.enabled;
    }
    public bool GiveFood()
    {
        if (HasFood())
        {
            foodItem.renderer.enabled = false;
            
            return true;
        }

        return false;
    }

    private void ChangeMaterial(string name)
    {
        name = name.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries)
            .Last()
            .Split('.')
            .First()
            .Trim();

        name = @"Assets\Materials\" + name + ".mat";

        Debug.Log(name);

        Material material = Resources.LoadAssetAtPath<Material>(name);

        foodItem.GetComponent<MeshRenderer>().material = material;
    }
    private void ChangeModel(string name)
    {
        Mesh mesh = Resources.LoadAssetAtPath<Mesh>(name);

        foodItem.GetComponent<MeshFilter>().mesh = mesh;
    }
    private void ChangeFood(string name)
    {
        ChangeModel(name);
        ChangeMaterial(name);
    }

    // Use this for initialization
    private void Start()
    {
        random = new Random();

        foodItem = GameObject.Find("FoodItem");

        foods = Directory.GetFiles(@"Assets\Model\");

        ChangeFood(foods[2]);
    }
	
	// Update is called once per frame
	private void Update()
    {
	}

    void OnTriggerEnter(Collider col)
    {
        if (ReferenceEquals(this.gameObject, col.gameObject))
        {
            return;
        }
        Debug.Log(col.gameObject.name);
        Debug.Log("HAISTA VITTU T GIGGI HIIRI");
    }
}
