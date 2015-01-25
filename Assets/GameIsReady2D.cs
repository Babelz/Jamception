using UnityEngine;
using System.Collections;

public class GameIsReady2D : MonoBehaviour {

    private float timer = 2f;
    private bool finish;
    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (!finish && player.GetComponent<QuestHUD>().questLog.CompletedQuests == 2)
        {
            timer -= Time.deltaTime;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<SpriteRenderer>().transform.position = player.transform.position;
            if (timer < 0)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                finish = true;
                GameObject.Find("PlayerCamera").GetComponent<CameraMovement>().EnterTheMatrix();
            }
        }
	}

}
