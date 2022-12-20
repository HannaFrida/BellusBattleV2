using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AI : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    private Vector3 direction;
    [SerializeField] private float checkDistance = 1;
    [SerializeField] private LayerMask layerM;
    [SerializeField] private float timetUntilDeath = 2f;
    private bool moving = true;
    public UnityEvent deathEvent;
    [SerializeField] Mesh mesh;
    public UnityEvent delayedDeathEvent;
    bool dead = false;

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
        if(moving)transform.position += direction * speed * Time.deltaTime;
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
        if(dead) return;
        dead = true;
        moving = false;
        deathEvent.Invoke();
        StartCoroutine(Wait());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            KillAI();
            Destroy(other.gameObject);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Despawn>().SetMesh(mesh);
        delayedDeathEvent.Invoke();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
               
}
