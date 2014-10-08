using UnityEngine;
using System.Collections;

public class Resource : MonoBehaviour {
    public float width;
    public float carriedResourceHeight;

	[HideInInspector] public OneDBox box = new OneDBox(0,8,0);
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate () {
        transform.position = new Vector3(box.x + (width / 2), transform.position.y, transform.position.z);
    }

    public void Pickup () {
        transform.position = new Vector3(box.x + (width / 2), transform.position.y + carriedResourceHeight, transform.position.z);
    }

    public void Drop () {
        transform.position = new Vector3(box.x + (width / 2), transform.position.y - carriedResourceHeight, transform.position.z);
    }
}
