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
}
