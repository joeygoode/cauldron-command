using UnityEngine;
using System.Collections;

public class OneDBox : MonoBehaviour {

    public float x;
    public float velocity;
    public bool immovable;
    public float width;

    public OneDBox (float x, float width, float velocity, bool immovable) {
        this.x = x;
        this.velocity = velocity;
        this.immovable = immovable;
        this.width = width;
    }
    
    public void FixedUpdate () {
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
    
    /* //probably not actually gonna need this kind of physics
    void separate(OneDBox box) {
        if (this.immovable && box.immovable) {
            return;
        }
        else if (this.x < box.x) {
            if (this.immovable) {
                box.x = this.x + this.width;
                return;
            }
            else if (box.immovable) {
                this.x = box.x - this.width;
                return;
            }
            else {
                float midDistance = (box.x - (this.x + this.width)) * 0.5f;
                float midpoint = this.x + this.width + midDistance;
            }
        }
        
    }*/
}
