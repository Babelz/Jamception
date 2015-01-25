using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform Target;


    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
            Vector3 point = camera.WorldToViewportPoint(Target.position);
            Vector3 delta = Target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);


            if (enterinMatrix)
            {
                float distCovered = (Time.time - startTime) * speed;
                float fracJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(start.position, (end.position), fracJourney);

                if (((end.position) - transform.position).sqrMagnitude < 0.05)
                {
                    var g = GameObject.Find("TileMap");
                    GameObject.DestroyImmediate(g); // no leaks
                    Application.LoadLevel("1D");
                }
            }

    }

    bool enterinMatrix = false;

    private Transform start;
    private Transform end;
    private float startTime;
    private float journeyLength;
    private float speed = 5f;

    public void EnterTheMatrix()
    {
        GameObject gobs = GameObject.Find("DestroyableLaptop");

        

        start = transform;
        end = gobs.transform;

        startTime = Time.time;
        journeyLength = Vector3.Distance(start.position, end.position);
        enterinMatrix = true;
    }
}
