using UnityEngine;
using System.Collections;

public class Resource : MonoBehaviour {
    public float carriedResourceHeight;
    public Sprite tombstone;
    public Sprite resource;
    public float width = 24;
    public GameObject mobPrefab;
    private PlayerController playerCarryingMe;
    private float groundY = 0;
    private float currentY;

    [HideInInspector]
    public int level = 0;

	[HideInInspector] public OneDBox box = new OneDBox(0,0,0);

    private SpriteRenderer sprender;
	// Use this for initialization
	void Start () {
        box.width = width;
        sprender = this.GetComponentInParent<SpriteRenderer>();
        sprender.sprite = tombstone;
        groundY = transform.position.y;
        currentY = groundY;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void setY(float y)
    {
        this.currentY = y;
    }

    private float getY()
    {
        if (playerCarryingMe == null)
        {
            return currentY;
        }
        else
        {
            return playerCarryingMe.getLiftYCoordinate();
        }
    }

    void FixedUpdate () {
        transform.position = new Vector3(box.x + (width / 2), getY(), transform.position.z);
    }

    public void Pickup (PlayerController player) {
        sprender.sprite = resource;
        playerCarryingMe = player;
    }

    public void Drop () {
        playerCarryingMe = null;
        transform.position = new Vector3(box.x + (width / 2), transform.position.y - carriedResourceHeight, transform.position.z);
        //this is kind of a subjective game design choice,
        //but right now I think dropped resources should be tokens, for ease of debugging and playablity
        //sprender.sprite = tombstone;
    }
}
