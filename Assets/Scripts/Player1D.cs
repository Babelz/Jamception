using UnityEngine;
using System.Collections;

public class Player1D : MonoBehaviour {

    public Sprite[] sprites;
    public Transform babiSprite;
    public GameObject prefab;

    private float speed = 5f;
    private GameObject[] computers;
    private StateManager stateManager;
    private bool hasAnIdea = false;
    private GameObject idea;

	// Use this for initialization
	void Start () {
        computers = GameObject.FindGameObjectsWithTag("Computer");
        stateManager = GameObject.FindGameObjectWithTag("StateManager").GetComponent<StateManager>();

        babiSprite = transform.Find("Sprite");
        babiSprite.transform.parent = transform;
	}

	// Update is called once per frame
	void Update () {
        if (stateManager.State == (int)GameStates.Normal)
        {
            float direction = Input.GetAxis("Horizontal");
            transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

            if (direction < 0)

                babiSprite.transform.localScale = new Vector2(-0.4f, 0.4f);
            else if (direction > 0)
            {
                babiSprite.transform.localScale = new Vector2(0.4f, 0.4f);
            }

            if (transform.position.x <= -9.5f)
            {
                transform.position = new Vector2(-9.5f, transform.position.y);
            }
            else if (transform.position.x >= 9.5f)
            {
                transform.position = new Vector2(9.5f, transform.position.y);
            }

            bool plsKillTheIdea = true;

            foreach (GameObject computer in computers)
            {
                if ((computer.transform.position - transform.position).sqrMagnitude < 0.25)
                {
                    Computer1D compComp = computer.GetComponent<Computer1D>();

                    if (compComp.IsBugged())
                    {
                        plsKillTheIdea = false;
                        if (!hasAnIdea)
                        {
                            hasAnIdea = true;
                            idea = GameObject.Instantiate(prefab, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.identity) as GameObject;

                            int spriteID = 0;

                            if (compComp.GetFixKey() == KeyCode.Z) spriteID = 0;
                            else if (compComp.GetFixKey() == KeyCode.X) spriteID = 1;
                            else if (compComp.GetFixKey() == KeyCode.C) spriteID = 2;
                            else if (compComp.GetFixKey() == KeyCode.V) spriteID = 3;

                            idea.GetComponent<SpriteRenderer>().sprite = sprites[spriteID];
                            idea.transform.localScale = new Vector2(0.4f, 0.4f);
                            idea.transform.parent = transform;
                        }

                        if (Input.GetKeyDown(compComp.GetFixKey()))
                        {
                            computer.renderer.material.color = Color.gray;
                            compComp.Fix();
                            plsKillTheIdea = true;
                        }
                        else if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.V))
                        {
                            compComp.AWildBSODAppears();
                        }
                    }
                }
            }

            if (plsKillTheIdea)
            {
                hasAnIdea = false;
                GameObject.Destroy(idea);
            }
        }
	}
}
