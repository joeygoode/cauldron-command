using UnityEngine;
using System.Collections.Generic;

public class Fort : MonoBehaviour {
    
    public int maxHitpoints = 100;    
    public float width = 32;
    public int faction = 0;
    public GameObject cauldronPrefab;

    //public List<Cauldron> cauldrons = new List<Cauldron>();
    
    [HideInInspector]
    public OneDBox box = new OneDBox(0,32, 0);

    [HideInInspector] public int hitpoints = 1;

	// Use this for initialization
	void Start () {
        box = new OneDBox(transform.position.x - width / 2, width, 0);
        hitpoints = maxHitpoints;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    
    void FixedUpdate () {
        box.FixedUpdate();
    }

    public void ResetHP () {
        hitpoints = maxHitpoints;
    }

    public bool IsDead () {
        return hitpoints <= 0;
    }
}
