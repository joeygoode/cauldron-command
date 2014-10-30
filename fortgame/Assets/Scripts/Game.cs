using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
    // Game instances
    public Team left;
    public Team right;
    public float resourceSpawnRate = 2;
    public float resourceSpawnVariance = 1;
    public int maxFreeResources = 30;
    public float resourceFortSpacing = 20;
    public GameObject background;
    public List<GameObject> resourceTypes;
    public GameObject combinationResource;

    private bool gameRunning = true;
    private float resourceSpawnCountdown;
    private List<Resource> resources = new List<Resource>();
	// Use this for initialization
	void Start () {
        ResetResourceCountdown();
        left.player.game = this;
        right.player.game = this;
	}
	
	// Update is called once per frame
	void Update () {
        float startButton = Input.GetAxisRaw("Start-Pause"); //has to be raw to work while paused
        if (startButton > 0.0)
        {
            Time.timeScale = 0;
            gameRunning = false;
        } 
        else if (left.fort.IsDead() || right.fort.IsDead()) {
            Time.timeScale = 0;
            gameRunning = false;
        }
        else
        {
            Time.timeScale = 1;
            gameRunning = true;
        }

        float resetButton = Input.GetAxisRaw("Reset");
        if (resetButton > 0.0) {
            Time.timeScale = 1;
            Application.LoadLevel("mainmenu");
        }

	}

    public bool IsValidMove (Team t, OneDBox b) {
        b.FixedUpdate();
        float width = background.GetComponent<SpriteRenderer>().sprite.rect.width;
        float center = background.GetComponent<Transform>().position.x;
        if ( b.x > center + width / 2 || b.x < -1 * (center + width / 2) )
        {
            return false;
        }
        else if (t.Equals(right))
        {
            return !left.fort.box.overlap(b);
        }
        else
        {
            return !right.fort.box.overlap(b);
        }
    }

    void ResetResourceCountdown () {
        resourceSpawnCountdown = Random.Range(resourceSpawnRate - resourceSpawnVariance,
                                              resourceSpawnRate + resourceSpawnVariance);
    }

    void SpawnResource () {
        float x = Random.Range(
            left.fort.box.x + left.fort.box.width + resourceFortSpacing, 
            right.fort.box.x - resourceFortSpacing);
        GameObject g = (GameObject)Instantiate(
            resourceTypes[Random.Range(0,resourceTypes.Count)], 
            new Vector3(x, 0, 0), 
            new Quaternion(0, 0, 0, 0));
        Resource r = g.GetComponent<Resource>();
        resources.Add(r);
    }

    public bool RemoveResource (PlayerController p) {
        Resource removedResource = null;
        foreach (Resource r in resources) {
            if (r.box.overlap(p.box)) {
                p.ReceiveResource(r);
                removedResource = r;
                break;
            }
        }
        if (removedResource != null) {
            resources.Remove(removedResource);
            return true;
        }
        return false;
    }

    public void AddResource (Resource r) {
        resources.Add(r);
    }

    void FixedUpdate () {
        if (gameRunning) {

            left.CollideWithFort(right.fort);
            right.CollideWithFort(left.fort);

            //collide mobs with each other
            foreach (Unit u1 in left.units)
            {
                if ( u1.box == null ) {
                    continue;
                }
                Mob m1 = (Mob) u1;
                foreach (Unit u2 in right.units)
                {
                    if( u2.box == null) {
                        continue;
                    }
                    Mob m2 = (Mob) u2;
                    if (m1.box.overlap(m2.box))
                    {
                        m1.targetMob = m2;
                        m2.targetMob = m1;
                        if (m1.box.velocity == 0)
                        {
                            m2.box.x = m1.box.x + m1.box.width;
                        }
                        if (m2.box.velocity == 0)
                        {
                            m1.box.x = m2.box.x - m1.box.width;
                        }
                    }
                }
            }

            List<Resource> deadResources = new List<Resource>();
            foreach (Resource r in resources)
            {
                //collide resource with altars
                if (r.level == 0)
                {
                    if (r.box.overlap(left.altar.box))
                    {
                        left.altar.recieveResource(r);
                        deadResources.Add(r);
                    }
                    else if (r.box.overlap(right.altar.box))
                    {
                        right.altar.recieveResource(r);
                        deadResources.Add(r);
                    }
                }
                //collide resource with cauldrons
                else
                {
                    //try left cauldrons or right cauldrons.
                    if (r.box.x < 0)
                    {
                        //only collide with the cauldrons on one floor.
                        if (left.fort.cauldrons.Count >= r.level)
                        {
                            foreach (Cauldron c in left.fort.cauldrons[r.level - 1])
                            {
                                if (r.box.overlap(c.box))
                                {
                                    c.recieveResource(r);
                                    deadResources.Add(r);
                                    //make sure the resource doesn't get put in multiple cauldrons, even if it overlaps with multiple cauldrons.
                                    r.box.x = float.MinValue;
                                }
                            }
                        }
                    }
                    else
                    {
                        //only collide with the cauldrons on one floor.
                        if (right.fort.cauldrons.Count >= r.level)
                        {
                            foreach (Cauldron c in right.fort.cauldrons[r.level - 1])
                            {
                                if (r.box.overlap(c.box))
                                {
                                    c.recieveResource(r);
                                    deadResources.Add(r);
                                    //make sure the resource doesn't get put in multiple cauldrons, even if it overlaps with multiple cauldrons.
                                    r.box.x = float.MaxValue;
                                }
                            }
                        }
                    }
                }
            }

            //collide resources with labs
            List<Resource> newResources = new List<Resource>();
            List<Team> teams = new List<Team>();
            teams.Add(left);
            teams.Add(right);
            foreach (Team t in teams)
            {
                int resCount = 0;
                Resource first = null;
                Resource second = null;
                foreach (Resource r in resources)
                {
                    if (r.box.overlap(t.fort.labBox))
                    {
                        if (resCount == 0)
                        {
                            Debug.Log("one");
                            first = r;
                            resCount++;
                        }
                        else if (resCount == 1)
                        {
                            Debug.Log("two");
                            second = r;
                            resCount++;
                        }
                    }
                }
                if (resCount == 2 && !t.player.box.overlap(t.fort.labBox))
                {
                    Debug.Log("combine!");
                    GameObject g = null;
                    if (t.direction < 0)
                    {
                        g = (GameObject)Instantiate(
                            combinationResource, 
                            new Vector3(t.fort.box.x + t.fort.box.width, transform.position.y, transform.position.z), 
                            new Quaternion(0, 0, 0, 0));
                    }
                    else
                    {
                        g = (GameObject)Instantiate(
                            combinationResource,
                            new Vector3(t.fort.box.x, transform.position.y, transform.position.z),
                            new Quaternion(0, 0, 0, 0));
                    }
                    newResources.Add(g.GetComponent<Resource>());
                    deadResources.Add(first);
                    deadResources.Add(second);
                }
            }

            foreach (Resource r in deadResources)
            {

                resources.Remove(r);
                Destroy(r.gameObject);
            }
            foreach (Resource r in newResources)
            {
                resources.Add(r);
            }
            //spawn resources
            if (resources.Count > maxFreeResources) {
            } else if (resourceSpawnCountdown < 0) {
                SpawnResource();
                ResetResourceCountdown();
            } else {
                resourceSpawnCountdown -= Time.fixedDeltaTime;
            }
        }
    }
}
