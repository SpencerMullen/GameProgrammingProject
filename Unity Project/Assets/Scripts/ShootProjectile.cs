using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootProjectile : MonoBehaviour
{
    public GameObject projectile;
    [SerializeField]
    private float projectileSpeed, shootRange;

    [SerializeField]
    private Camera fpsCam;

    Rigidbody rb;

    //public AudioClip spellSFX;
    //public Image crossHair;
    //public Color reticleDementorColor;
    //Color originalReticleColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //originalReticleColor = crossHair.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //Shoots();
        }
    }

    // Shoot using physics
    void Shoot_physics()
    {
        GameObject project = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
        var _rb = project.GetComponent<Rigidbody>();
        _rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);
    }

    //void Shoot_Ray()
    //{
    //    RaycastHit hit;
    //    // first person
    //    if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, shootRange))
    //    {
    //        // TODO hit enemy
    //        Debug.Log(hit.transform.position);
    //        //hit.transform.GetComponent<>();
    //    }
    //}



    //private void FixedUpdate()
    //{
    //    ReticleEffect();
    //}

    //void ReticleEffect()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
    //    {
    //        if (hit.collider.CompareTag("Dementor"))
    //        {
    //            crossHair.color = Color.Lerp(crossHair.color, reticleDementorColor, Time.deltaTime * 2);
    //            crossHair.transform.localScale = Vector3.Lerp(crossHair.transform.localScale, new Vector3(0.7f, 0.7f, 1), Time.deltaTime * 2);
    //        }
    //        else
    //        {
    //            crossHair.color = Color.Lerp(crossHair.color, reticleDementorColor, Time.deltaTime * 2);
    //            crossHair.transform.localScale = Vector3.Lerp(crossHair.transform.localScale, Vector3.one, Time.deltaTime * 2);
    //        }
    //    }
    //}
}
