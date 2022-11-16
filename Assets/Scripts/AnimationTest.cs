using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{

    private Animator anim;
    private bool movingRight = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) anim.SetTrigger("Jump");
        float horizontal = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", horizontal);

        if (movingRight && horizontal < 0)
        {
            transform.Rotate(new Vector3(0, -180, 0));
            movingRight = false;
        }

        if (!movingRight && horizontal > 0)
        {
            transform.Rotate(new Vector3(0, 180, 0));
            movingRight = true;
        }
    }



}
