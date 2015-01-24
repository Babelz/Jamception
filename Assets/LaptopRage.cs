using UnityEngine;
using System.Collections;
public delegate void LaptopRageEventHandler(LaptopRage sender);
public class LaptopRage : MonoBehaviour {

    private bool flipTheLaptopInitiated = false;

    public event LaptopRageEventHandler OnFlippingFinished;

    public bool CanBeFlipped
    {
        get
        {
            return !flipTheLaptopInitiated && !CantBeRotated;
        }
    }

    public bool CantBeRotated
    {
        get
        {
            return transform.rotation.x > 0.82f && transform.rotation.x < 0.9f;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
       
        if (!flipTheLaptopInitiated) return;

        transform.Rotate(Vector3.right * 5f);
        
        
        if (CantBeRotated)
        {
            flipTheLaptopInitiated = false;
            if (OnFlippingFinished != null)
                OnFlippingFinished(this);
        }

	}

    public void InitiateRage()
    {
        if (!CanBeFlipped) return;
        flipTheLaptopInitiated = true;
    }
}
