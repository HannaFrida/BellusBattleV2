using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{


    public GameObject Gun;

    public GameObject Bullet;
    public GameObject Beam;

    public float bulletSpeed;

    public Transform ShootPoint;

    public float fireRate;

    public Camera mainCamera;

    float fireAgain;

    bool beamReady = false;

    public float timeUntilBeam = 1.7f;

    public float ammo = 6;

    GameObject bulletIns;
    GameObject beamIns;

    // Start is called before the first frame update

    public void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {

        if(ammo <= 0){
            gameObject.GetComponent<Sword_Prototype>().enabled = true;
            gameObject.GetComponent<Shoot>().enabled = false;
        }
            
        faceGun();

        if (ammo > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
            
                if(Time.time > fireAgain)
                {
                    fireAgain = Time.time + 1/fireRate;
                    shoot();
                }
            
            }
        }
        

        if (beamReady == true)
        {
            timeUntilBeam -= Time.deltaTime;
            if(timeUntilBeam <= 0)
            {
                beamIns = Instantiate(Beam, ShootPoint.position, ShootPoint.rotation);
                beamIns.transform.Rotate(Vector3.left * 180);
                Destroy(beamIns, 0.5f);
                timeUntilBeam = 1.7f;
                beamReady = false;
            }
            
        }
    }
     void faceGun()
    {
        float camToPlayerDist = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camToPlayerDist));
        Vector2 direction = mouseWorldPosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Gun.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        if(bulletIns != null) {
            bulletIns.transform.position = ShootPoint.position;
            bulletIns.transform.rotation = Quaternion.Euler(0, 0, angle + 90); 
        }
        if (beamIns != null)
        {
            beamIns.transform.position = ShootPoint.position;
            beamIns.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        }


    }
    public void eqiup(GameObject pickup)
    {
        Gun = pickup;
        
    }
    public void die()
    {
        Destroy(gameObject, 0.5f);
        
    }
    void shoot()
    {
        ammo = ammo -1;
        bulletIns = Instantiate(Bullet, ShootPoint.position, ShootPoint.rotation);

        bulletIns.transform.Rotate(Vector3.left * 180);
       // Rigidbody bulletIns_rig;
       // bulletIns_rig = bulletIns.GetComponent<Rigidbody>();
       // bulletIns_rig.AddForce(ShootPoint.transform.up * bulletSpeed);
        beamReady = true;
        Destroy(bulletIns, 1.9f);

    }

    
}
