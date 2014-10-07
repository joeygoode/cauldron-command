using UnityEngine;
using System.Collections;

enum PlayerState {
    Idle,
    PickUp,
    Carry,
    PutDown
}

public class PlayerController : MonoBehaviour {

    private float walkSpeed = 50.0f;
    private float width = 10;

    public string walkAxis = "P1Horizontal";
    public string digButton = "P1Dig";

    [HideInInspector]
    public OneDBox box;
    private Animator animator;
    private PlayerState state = PlayerState.Idle;
    private Resource heldResource;

    // Use this for initialization
    void Start () {
        box = new OneDBox(transform.position.x - (width / 2), width, 0); 
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if (state == PlayerState.Idle || state == PlayerState.Carry)
        {
            //input
            float walkDir = Input.GetAxis(walkAxis);
            //physics
            box.velocity = walkDir * walkSpeed;
            //animation
            if (walkDir != 0)
            {
                animator.SetBool("isWalking", true);
                if (walkDir < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                } else if (walkDir > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            } else
            {
                animator.SetBool("isWalking", false);
            }
        }
        else {
            animator.SetBool("isWalking", false);
        }
    }

    public void ReceiveResource(Resource r) {
        heldResource = r;
        state = PlayerState.Carry;
        heldResource.Pickup();
    }

    //Fixed Update is called at a fixed timestep
    void FixedUpdate () {
        box.FixedUpdate();
        transform.position = new Vector3(box.x + (width / 2), transform.position.y, transform.position.z);
    }
}
