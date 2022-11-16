using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw_Grenade_prototype : MonoBehaviour


{


    public GameObject Gun;

    public GameObject Bullet;

    public float bulletSpeed;

    public Transform ShootPoint;

    public float fireRate;

    public Camera mainCamera;

    float fireAgain;

    public float ammo = 6;

    bool pinOut;

    bool tro;

    Rigidbody bulletIns_rig;

    GameObject bulletIns;



    // Start is called before the first frame update

    public void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {

        faceGun();

        

        if (ammo > 0)
        {
            if (Input.GetMouseButton(0))
            {
                

                if (Time.time > fireAgain)
                {
                    if (!pinOut)
                    {
                        fireAgain = Time.time + 1 / fireRate;
                        Pin();
                        
                    }
                    
                    
                }
                

            }if (Input.GetMouseButtonUp(0))
            {
                        thro();
            }
        }
        if (pinOut && !tro)
        {
            bulletIns.transform.position = ShootPoint.position;
        }
        
            
        

    }
    void faceGun()
    {
        float camToPlayerDist = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camToPlayerDist));
        Vector2 direction = mouseWorldPosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Gun.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }
    public void eqiup(GameObject pickup)
    {
        Gun = pickup;

    }
    public void die()
    {
        Destroy(gameObject, 0.5f);
    }
    void Pin()
    {
        ammo = ammo - 1;
        bulletIns = Instantiate(Bullet, ShootPoint.position, ShootPoint.rotation);
        bulletIns.transform.Rotate(Vector3.left * 90);
        
        bulletIns_rig = bulletIns.GetComponent<Rigidbody>();
        pinOut = true;
        tro = false;

    }
    void thro(){
        tro = true;
        pinOut = false;
        bulletIns_rig.AddForce(ShootPoint.transform.up * bulletSpeed);
        Destroy(bulletIns, 5f);
        
    }
}
