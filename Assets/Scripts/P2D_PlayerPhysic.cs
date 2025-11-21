using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2D_PlayerPhysic : MonoBehaviour
{
    protected float gravity = 9.81f;
    public Vector2 velocity;


    private void FixedUpdate()
    {
        ApplyVelocity();
        ApplyGravity();
    
    }

    private void ApplyVelocity()
    {
        transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.fixedDeltaTime;
    }

    private void ApplyGravity()
    {
        if (!IsGrounded())
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
        }
        else
        {
            if (velocity.y < 0)
                velocity.y = 0; // stop sinking
        }
    }

    virtual protected bool IsGrounded()
    {
        Debug.LogWarning("P2D_Physic: IsGrounded() should be overrided by P2D_Rigidbody, otherise it may cause some problems");
        return false;
    }
}
