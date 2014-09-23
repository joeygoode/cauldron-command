using UnityEngine;
using System.Collections;

public class Fort : MonoBehaviour {
    
    public int hitpoints = 100;    
    public float width = 32;

    public Mob[] Mobs = new Mob[128];
    
    [HideInInspector]
    public OneDBox box;
    
	// Use this for initialization
	void Start () {
        box = new OneDBox(transform.position.x, width, 0, true);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    
    void FixedUpdate () {
        
    }
}
