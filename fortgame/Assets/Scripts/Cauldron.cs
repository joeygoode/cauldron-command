using UnityEngine;
using System.Collections;

public class Cauldron : MonoBehaviour {

    public float width = 20;
    public GameObject archetypeMob;
    public float spawnRate = 1.0f;
    public int faction = 0;

    [HideInInspector]
    public OneDBox box;
    private float spawnTimer = 0;
    private bool spawnThisFrame = false;

	// Use this for initialization
	void Start () {
        box = new OneDBox(transform.position.x - width / 2, width, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        if (archetypeMob != null)
        {
            spawnTimer -= Time.fixedDeltaTime;
            if (spawnTimer < 0)
            {
                spawnTimer = spawnRate;
                spawnThisFrame = true;
            }
        }

        box.FixedUpdate();
    }

    public bool getSpawn()
    {
        if (spawnThisFrame)
        {
            spawnThisFrame = false;
            return true;
        }
        return false;
    }
}
