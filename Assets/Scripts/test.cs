using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 _velocity;

    public float peakPosY;
    private float temp_peakPosY;
    public float jumpHeight;
    private float jumpStartY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {
            jumpStartY = transform.position.y;
        }

    }

    private void FixedUpdate()
    {
        _velocity = rb.velocity;
        temp_peakPosY = Mathf.Max(transform.position.y, temp_peakPosY);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            jumpHeight = temp_peakPosY - jumpStartY;
            peakPosY = temp_peakPosY;
            Debug.Log("Peak Y: " + temp_peakPosY);
            temp_peakPosY = 0f;
        }
    }
}
