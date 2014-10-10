using UnityEngine;
using System.Collections.Generic;

public class SacrificialAltar : MonoBehaviour {

    public float width = 10;
    public int spawnAmount = 5;
    public float spawnTime = 0.5f;
    [HideInInspector]
    public OneDBox box;
    public Team team;
    private float spawnTimer = 0.1f;
    private Queue<Resource> mobQueue;

	// Use this for initialization
	void Start () {
        box = new OneDBox(transform.position.x - (width / 2), width, 0);
        mobQueue = new Queue<Resource>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void FixedUpdate()
    {
        if (mobQueue.Count > 0)
        {
            spawnTimer -= Time.fixedDeltaTime;
            if (spawnTimer < 0)
            {
                spawnTimer = spawnTime;
                team.SpawnMob(mobQueue.Dequeue());
            }
        }
    }

    public void recieveResource(Resource resource)
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            mobQueue.Enqueue(resource);
        }
    }
}
