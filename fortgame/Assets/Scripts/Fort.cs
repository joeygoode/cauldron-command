using UnityEngine;
using System.Collections.Generic;

public class Fort : MonoBehaviour {
    
    public int maxHitpoints = 100;    
    public float width = 32;
    public int direction = 1;
    public GameObject cauldronPrefab;
    public GameObject floorPrefab;
    public int floors = 0;
    public int maxFloors = 5;
    public int floorProgress = 0;
    public int floorProgressRequired = 100;
    private List<GameObject> floorObjects;
    public GameObject roof;
    public float floorHeight = 50;
    public float baseHeight = 100;
    public float floorXOffset = 30;

    //public List<Cauldron> cauldrons = new List<Cauldron>();
    
    [HideInInspector]
    public OneDBox box = new OneDBox(0,32, 0);

    [HideInInspector] public int hitpoints = 1;

	// Use this for initialization
	void Start () {
        box = new OneDBox(transform.position.x - width / 2, width, 0);
        hitpoints = maxHitpoints;
        floorObjects = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 thisP = this.transform.position;
	    //update floor sprites
        if(floorObjects.Count < floors) {
            Vector3 floorPos = new Vector3(thisP.x + floorXOffset * direction, thisP.y + baseHeight + floorHeight * floorObjects.Count, thisP.z);
            GameObject floorObj = (GameObject)Instantiate(floorPrefab, floorPos, this.transform.rotation);
            floorObj.GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;
            floorObjects.Add(floorObj);
        }
        if(floorObjects.Count > floors) {
            Destroy(floorObjects[floorObjects.Count - 1]);
            floorObjects.RemoveAt(floorObjects.Count - 1);
        }
        //update roof sprite
        roof.transform.position = new Vector3(thisP.x + floorXOffset * direction, thisP.y + floorHeight * (floorObjects.Count + 1), thisP.z);
	}
    
    void FixedUpdate () {
        box.FixedUpdate();
        //auto-gen floors until we put in poltergeists
        floorProgress++;
        //build new floors
        if (floorProgress >= floorProgressRequired)
        {
            floorProgress -= floorProgressRequired;
            floors++;
            if (floors > maxFloors)
            {
                floors = maxFloors;
            }
        }
    }

    public void ResetHP () {
        hitpoints = maxHitpoints;
    }

    public void setColor(Color c)
    {
        this.GetComponent<SpriteRenderer>().color = c;
        roof.GetComponent<SpriteRenderer>().color = c;
        foreach (GameObject floorSprite in floorObjects) {
            floorSprite.GetComponent<SpriteRenderer>().color = c;
        }
    }

    public bool IsDead () {
        return hitpoints <= 0;
    }
}
