using UnityEngine;
using System.Collections.Generic;

public class Fort : MonoBehaviour {
    
    public int maxHitpoints = 100;    
    public float width = 32;
    [HideInInspector]
    public int direction = 1;
    public GameObject cauldronPrefab;
    public GameObject floorPrefab;
    public int floors = 0;
    public int maxFloors = 5;
    public int floorProgress = 0;
    public int floorProgressRequired = 100;
    private List<GameObject> floorObjects = new List<GameObject>();
    public GameObject roof;
    public float floorHeight = 50;
    public float baseHeight = 100;
    public float floorXOffset = 30;
    public float labPercentage;
    public GameObject CombinationResource;

    //public List<Cauldron> cauldrons = new List<Cauldron>();
    
    [HideInInspector]
    public OneDBox box = new OneDBox(0,32, 0);
    [HideInInspector]
    public OneDBox labBox = new OneDBox(0,32,0);
    [HideInInspector]
    public Team team;
    [HideInInspector] public int hitpoints = 1;

	// Use this for initialization
	void Start () {
        box = new OneDBox(transform.position.x - width / 2, width, 0);
        float spriteWidth = GetComponent<SpriteRenderer>().sprite.rect.width;
        float leftEdge;
        if (team.direction < 0)
        {
            leftEdge = transform.position.x - spriteWidth / 2 + (1 - labPercentage) * spriteWidth;
        }
        else
        {
            leftEdge = transform.position.x - spriteWidth / 2;
        }
        labBox = new OneDBox(leftEdge, spriteWidth * labPercentage, 0);
        hitpoints = maxHitpoints;
	}
	
    public Resource Combine (Resource first, Resource Second) {
        float newX;
        if (team.direction < 0)
        {
            newX = labBox.x;
        }
        else
        {
            newX = box.x;
        }
        GameObject g = (GameObject) Instantiate(CombinationResource, new Vector3(newX, 0, 0), new Quaternion(0, 0, 0, 0));
        return g.GetComponent<Resource>();
    }

	// Update is called once per frame
	void Update () {
        Vector3 thisP = this.transform.position;
	    //update floor sprites
        if(floorObjects.Count < floors) {
            Vector3 floorPos = new Vector3(thisP.x + floorXOffset * direction, thisP.y + baseHeight + floorHeight * floorObjects.Count, thisP.z);
            GameObject floorObj = (GameObject)Instantiate(floorPrefab, floorPos, this.transform.rotation);
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

    public bool IsDead () {
        return hitpoints <= 0;
    }
}
