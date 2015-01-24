using UnityEngine;
using System.Collections;

public class Status1D : MonoBehaviour {

    private float scoreMax = 100f;
    private float score = 0f;
    private float maxScale = 5f;

	// Use this for initialization
	void Start () {
        renderer.material.color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = new Vector3(maxScale * (score / scoreMax), transform.localScale.y, transform.localScale.z);
	}

    public void addScore(float amount)
    {
        score += amount;

        if (score > scoreMax) score = scoreMax;
    }
}
