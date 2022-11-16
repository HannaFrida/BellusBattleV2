using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    private Vector3 direction;
    [SerializeField] private float checkDistance = 1;
    [SerializeField] private LayerMask layerM;
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
    }
    private void FixedUpdate()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    private void CheckCollision()
    {
        if (Physics.BoxCast(transform.position, transform.localScale, direction, Quaternion.identity, checkDistance, layerM))
        {
            if (direction == Vector3.right) direction = Vector2.left;
            else direction = Vector2.right;
        }
    }
    public void KillAI()
    {
        Destroy(gameObject);
    }
}
