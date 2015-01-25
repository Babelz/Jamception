using UnityEngine;
using System.Collections;

public class StatusBehaviour : MonoBehaviour
{
    #region Vars
    public int hunger;
    public int thirst;
    public int fatigue;
    public string ownerName;
    #endregion

    // Use this for initialization
	private void Start() 
	{
	}
	
	// Update is called once per frame
	private void Update() 
    {
	}

    public void Slap()
    {
        fatigue -= 5;
    }
    public void GiveDrink()
    {
        thirst -= 5;
    }
    public void GiveFood()
    {
        hunger -= 5;
    }

    public bool Fainted()
    {
        return fatigue > 30 && (thirst > 45 || hunger > 45);
    }

    public int Code()
    {
        return 100 - (fatigue + thirst + hunger);
    }

    public string[] ToStringArray()
    {
        return new string[]
        {
            "Name: " + ownerName,
            "Fatigue: " + fatigue,
            "Hunger: " + hunger,
            "Thirst: " + thirst
        };
    }
}
