using UnityEngine;
using System.Collections.Generic;

public class Fort : MonoBehaviour {
    
    public int hitpoints = 100;    
    public float width = 32;
    public int faction = 0;
    public GameObject cauldronPrefab;

    public List<Mob> mobs = new List<Mob>();
    public List<Cauldron> cauldrons = new List<Cauldron>();
    
    [HideInInspector]
    public OneDBox box = new OneDBox(0,32, 0);

    private int actualHP;

	// Use this for initialization
	void Start () {
        box = new OneDBox(transform.position.x - width / 2, width, 0);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    
    void FixedUpdate () {
        box.FixedUpdate();
    }

    public void ResetHP () {
        actualHP = hitpoints;
    }
}
