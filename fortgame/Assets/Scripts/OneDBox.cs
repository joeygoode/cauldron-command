using UnityEngine;
using System.Collections;

public class OneDBox : MonoBehaviour {

    public float x;
    public float velocity;
    public bool immovable;
    public float width;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void FixedUpdate () {
        if (!immovable) {
            x += velocity * Time.fixedDeltaTime;
        }
    }
    
    bool overlap (OneDBox box) {
        if (this.x > box.x) {
            //overlap right
            if (this.x < box.x + box.width) {
                return true;
            }
            //overlap enveloped
            if (this.x + this.width < box.x + box.width) {
                return true;
            } 
            //too far right
            return false;
        }
        //overlap left
        if (this.x + this.width > box.x) {
            return true;
        }
        //overlap enveloping
        if (this.x + this.width > box.x + box.width) {
            return true;
        }
        return false;
    }
    
    void separate(OneDBox box) {
        if (this.immovable && box.immovable) {
            return;
        }
        if (this.x < box.x) {
            
        }
    }
}
