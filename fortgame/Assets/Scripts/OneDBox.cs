using UnityEngine;
using System.Collections;

public class OneDBox {

    public float x;
    public float velocity;
    public float width;

    public OneDBox (float x, float width, float velocity) {
        this.x = x;
        this.velocity = velocity;
        this.width = width;
    }

    public void FixedUpdate () {
        x += velocity * Time.fixedDeltaTime;
    }

    public bool overlap (OneDBox box) {
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
}
