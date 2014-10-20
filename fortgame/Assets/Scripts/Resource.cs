using UnityEngine;
using System.Collections;

public class Resource : MonoBehaviour {
    public float carriedResourceHeight;
    public Sprite tombstone;
    public Sprite resource;
    public float width = 24;

	[HideInInspector] public OneDBox box = new OneDBox(0,0,0);

    private SpriteRenderer sprender;
	// Use this for initialization
	void Start () {
        box.width = width;
        sprender = this.GetComponentInParent<SpriteRenderer>();
        sprender.sprite = tombstone;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate () {
        transform.position = new Vector3(box.x + (width / 2), transform.position.y, transform.position.z);
    }

    public void Pickup () {
        transform.position = new Vector3(box.x + (width / 2), transform.position.y + carriedResourceHeight, transform.position.z);
        sprender.sprite = resource;
    }

    public void Drop () {
        transform.position = new Vector3(box.x + (width / 2), transform.position.y - carriedResourceHeight, transform.position.z);
        //this is kind of a subjective game design choice,
        //but right now I think dropped resources should be tokens, for ease of debugging and playablity
        //sprender.sprite = tombstone;
    }
}
