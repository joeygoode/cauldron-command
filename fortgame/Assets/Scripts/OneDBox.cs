using UnityEngine;
using System.Collections;

public class OneDBox {

    public float x;
    public float velocity;
    public float width;
    public int layer;

    public OneDBox (float x, float width, float velocity) {
        this.x = x;
        this.velocity = velocity;
        this.width = width;
        this.layer = 0;
    }

    public OneDBox (float x, float width, float velocity, int layer) {
        this.x = x;
        this.velocity = velocity;
        this.width = width;
        this.layer = layer;
    }

    public void FixedUpdate () {
        x += velocity * Time.fixedDeltaTime;
    }

    public bool overlap (OneDBox box) {
        if (this.layer != box.layer)
        {
            return false;
        }
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

    public float overlapAmount(OneDBox box)
    {
        if (this.x > box.x)
        {
            //overlap enveloped
            if (this.x + this.width < box.x + box.width)
            {
                if (this.width < box.width) { return this.width; }
                else { return box.width; }
            }
            //overlap right
            if (this.x < box.x + box.width)
            {
                return box.x + box.width - this.x;
            }
            //too far right
            return 0;
        }
        //overlap enveloping
        if (this.x + this.width > box.x + box.width)
        {
            if (this.width < box.width) { return this.width; }
            else { return box.width; }
        }
        //overlap left
        if (this.x + this.width > box.x)
        {
            return this.x + this.width - box.x;
        }
        return 0;
    }
}

