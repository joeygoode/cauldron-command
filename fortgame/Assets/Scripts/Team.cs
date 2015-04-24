using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Team : MonoBehaviour {
    //strings for getting the game objects placed in the scene
    public Color color = Color.red;
    public int direction;
    // Ugly, need a better way to do this
    public GameObject enemyPrefab;

    public List<Unit> units = new List<Unit>();
    //GameObjects in the scene
    public GameObject fortObj;
    public GameObject playerObj;
    public GameObject scoreObj;
    public GameObject altarObj;
    //Fort script objects
    [HideInInspector] 
    public Fort fort;
    //Character script objects
    [HideInInspector] 
    public PlayerController player;
    //Text display objects
    private GUIText score;
    //altar object
    [HideInInspector] 
    public SacrificialAltar altar;

    //this initializes before start
    void Awake()
    {
        fort = fortObj.GetComponent<Fort>();
        player = playerObj.GetComponent<PlayerController>();
        player.team = this;
        fort.team = this;
        score = scoreObj.GetComponent<GUIText>();
        altar = altarObj.GetComponent<SacrificialAltar>();
    }

	// Use this for initialization
	void Start () {
        fort.direction = direction;
	}

    // Update is called once per frame
    void Update () {
        if (fort.hitpoints <= 0)
        {
            score.text = "YOU LOSE";
        } 
        else
        {
            score.text = "HP: " + fort.hitpoints + " / " + fort.maxHitpoints;
        }
    }

    // Fixed Update is called 
    void FixedUpdate()
    {
        StackMobs();
        RemoveDead();
    }

    private void StackMobs()
    {
        //collide mobs with themselves, and
        // stack them in an animated way based on their position relative to their width/height

        //reset stacking vars
        foreach (Unit u in units)
        {
            Mob m = u as Mob;
            if (m == null) {
                continue;
            }
            m.squish = 0;
            m.y = 0;
        }

        //convert to array to iterate over
        Unit[] mobArr = units.ToArray();
        //collide every mob with every mob (except itself)
        //and only do it if there actually is more than 1 mob
        if (mobArr.Length > 1)
        {
            for (int i = 0; i < mobArr.Length - 1; i++)
            {
                if ( mobArr[i].box == null ) {
                    continue;
                }
                Mob m1 = (Mob) mobArr[i];
                for (int j = i + 1; j < mobArr.Length; j++)
                {
                    if ( mobArr[j].box == null) {
                        continue;
                    }
                    Mob m2 = (Mob) mobArr[j];
                    if (mobArr[i].box.overlap(mobArr[j].box))
                    {
                        //calculate ratio for squishiness/height boost
                        bool iThin = true;
                        float thinner = m1.width;
                        if (m2.width < thinner)
                        {
                            thinner = m2.width;
                            iThin = false;
                        }
                        float overlapRatio = mobArr[i].box.overlapAmount(mobArr[j].box) / thinner;
                        //thinner mob goes on top
                        Mob thinMob;
                        Mob thickMob;
                        if (iThin)
                        {
                            thinMob = m1;
                            thickMob = m2;
                        }
                        else
                        {
                            thinMob = m2;
                            thickMob = m1;
                        }
                        //set squishiness only if we are the "heaviest" thing squishing the mob.
                        //"heaviest" meaning "thing that's squishing it the most"
                        if (overlapRatio > m1.squish)
                        {
                            thickMob.squish = overlapRatio * thickMob.squishiness;
                        }
                        thinMob.y += overlapRatio * thickMob.height * (1 - thickMob.squishiness);
                    }
                }
            }
        }
    }

    public void SpawnMob(Resource resource)
    {
        Transform t = fort.GetComponentInParent<Transform>();
        GameObject g = (GameObject)Instantiate(resource.mobPrefab, t.position, new Quaternion(0, 0, 0, 0));
        Unit u = g.GetComponent<Unit>();
        u.team = this;
        units.Add(u);
        //u.PlaySound();
    }

    public void CollideWithFort (Fort f) {
        foreach (Unit u in units)
        {
            if (u.box == null) {
                continue;
            }
            Mob m = (Mob) u;
            if (m.box.overlap(f.box))
            {
                f.hitpoints -= m.fortDamage;
                m.hitPoints = 0;
            }
        }
    }

    public void RemoveDead () {
        //destroy any mobs with 0 hitpoints
        List<Unit> deadMobBuffer = new List<Unit>();
        foreach (Unit u in units)
        {
            if(u.hitPoints <= 0)
            {
                deadMobBuffer.Add(u);
            }
        }
        foreach (Unit u in deadMobBuffer)
        {
            units.Remove(u);
            Destroy(u.gameObject);
        }
    }
}
