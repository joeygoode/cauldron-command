﻿using UnityEngine;
using System.Collections.Generic;

public class Fort : MonoBehaviour {
    
    public int maxHitpoints = 150;
    public int baseHitpoints = 150;
    public int hitpointsPerFloor = 100;
    public int hitpointsPerLategameFloor = 30;
    public float width = 32;
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
	public float labWidth = 180;
    public float floorWidth = 262;
    public float cauldronXStart = 80;
    public float doorXPos = 192;
    public float doorWidth = 37;
    [HideInInspector]
    public OneDBox doorBox = new OneDBox(0, 0, 0);
    [HideInInspector]
    public OneDBox floorBox = new OneDBox(0, 0, 0);

    private int constructionTicker = 0;

    public int cauldronsPerFloor = 2;
    public float cauldronSpawnRate = 3.0f;
    private float cauldronTimer = 0;
    [HideInInspector]
    public List<List<Cauldron>> cauldrons = new List<List<Cauldron>>();
    
    [HideInInspector]
    public OneDBox box = new OneDBox(0,0, 0);

	[HideInInspector]
    public OneDBox labBox = new OneDBox(0,0,0);
    [HideInInspector] public int hitpoints = 1;

    public Team team;

	// Use this for initialization
	void Start () {
        maxHitpoints = baseHitpoints;
        box = new OneDBox(transform.position.x - width / 2, width, 0);
        float spriteWidth = GetComponent<SpriteRenderer>().sprite.rect.width;

        if (direction == 1)
        {
            labBox = new OneDBox(box.x, labWidth, 0);
        }
        else
        {
            labBox = new OneDBox(box.x + box.width - labWidth, labWidth, 0);
        }
        hitpoints = maxHitpoints;
        if (direction == 1)
        {
            doorBox = new OneDBox(box.x + width - doorXPos - doorWidth, doorWidth, 0);
        }
        else
        {
            doorBox = new OneDBox(box.x + doorXPos, doorWidth, 0);
        }

        floorBox = new OneDBox(box.x + box.width/2 + floorXOffset * direction - floorWidth/2, floorWidth, 0);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 thisP = this.transform.position;
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

        //update hitpoints
        maxHitpoints = baseHitpoints + floors * hitpointsPerFloor;
        if (hitpoints > maxHitpoints)
        {
            hitpoints = maxHitpoints;
        }

        //update floor sprites
        if (floorObjects.Count < floors)
        {
            Vector3 thisP = this.transform.position;
            //add a floor
            Vector3 floorPos = new Vector3(thisP.x + floorXOffset * direction, thisP.y + baseHeight + floorHeight * floorObjects.Count, thisP.z);
            GameObject floorObj = (GameObject)Instantiate(floorPrefab, floorPos, this.transform.rotation);
            floorObjects.Add(floorObj);
            //add hitpoints
            if (constructionTicker < maxFloors)
            {
                hitpoints += hitpointsPerFloor;
                constructionTicker++;
            }
            else
            {
                hitpoints += hitpointsPerLategameFloor;
            }
            //add cauldrons on the floor
            cauldrons.Add(new List<Cauldron>());
            for (int i = 0; i < cauldronsPerFloor; i++)
            {
                //calculate cauldron position
                float cauldronX = 0;
                float cauldronXSpace = floorWidth - floorXOffset;
                if (direction == 1)
                {
                    cauldronX = floorBox.x + floorXOffset + cauldronXSpace * (((float)i + 1) / ((float)cauldronsPerFloor + 1));
                }
                else
                {
                    cauldronX = floorBox.x + floorWidth - floorXOffset - cauldronXSpace * (((float)i + 1) / ((float)cauldronsPerFloor + 1));
                }
                //create and add object
                Vector3 cauldronPos = new Vector3(cauldronX, floorPos.y, thisP.z);
                GameObject cauldronObj = (GameObject)Instantiate(cauldronPrefab, cauldronPos, this.transform.rotation);
                cauldrons[floors - 1].Add(cauldronObj.GetComponent<Cauldron>());
            }

        }

        //check for floor death
        if (hitpoints < baseHitpoints + (floors - 1) * hitpointsPerFloor)
        {
            if (floors > 0)
            {
                floors--;
            }
        }

        if (floorObjects.Count > floors)
        {
            //destroy floor sprites
            Destroy(floorObjects[floorObjects.Count - 1]);
            floorObjects.RemoveAt(floorObjects.Count - 1);
            //destroy cauldrons
            foreach (Cauldron c in cauldrons[floorObjects.Count])
            {
                if (c.hasResource)
                {
                    c.resource.Destroy();
                }
                c.Destroy();
            }
            cauldrons.RemoveAt(floorObjects.Count);
        }

        //cauldron spawning
        cauldronTimer += Time.fixedDeltaTime;
        if (cauldronTimer >= cauldronSpawnRate)
        {
            cauldronTimer -= cauldronSpawnRate;
            foreach (List<Cauldron> lc in cauldrons)
            {
                foreach (Cauldron c in lc)
                {
                    if (c.hasResource)
                    {
                        team.SpawnMob(c.resource);
                    }
                }
            }
        }
    }

    public void ResetHP () {
        hitpoints = maxHitpoints;
    }

    public bool IsDead () {
        return hitpoints <= 0;
    }

    public int requestStairs(int level, OneDBox igorBox)
    {
        if (!igorBox.overlap(doorBox))
        {
            return -1;
        }
        if (level > floors)
        {
            return floors;
        }
        if (level <= 0)
        {
            return 0;
        }
        return level;
    }

    public float getFloorHeight(int level)
    {
        if (level == 0)
        {
            return this.transform.position.y;
        }
        return (level - 1) * floorHeight + baseHeight + this.transform.position.y;
    }
}
