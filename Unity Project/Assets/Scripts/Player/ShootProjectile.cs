using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;


public class ShootProjectile : MonoBehaviour
{
    public static ShootProjectile Instance;

    [Header("Projectile Settings")]
    public GameObject projectile;
    [SerializeField]
    private float projectileDamage = 20;
    [SerializeField]
    private float projectileSpeed, shootRange;
    public Transform shootPos;

    public bool UseRayShoot;
    public bool enableKnockBack;
    public float knockbackForce;
    public float fireRate = 15f;
    private float nextFire = 0f;

    [SerializeField]
    private Camera fpsCam;

    Rigidbody rb;
    [Header("Fire effects")]
    public ParticleSystem flashEffect;
    public GameObject impactOnHit;

    //public AudioClip spellSFX;
    public Image crossHair;

    //public Color reticleDementorColor;
    //Color originalReticleColor;

    private GunRecoil recoil;

    public static bool canShoot;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();
        recoil = GetComponent<GunRecoil>();

    }

    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;

        //originalReticleColor = crossHair.color;
    }

    private void FixedUpdate()
    {
        ReticleEffect();
    }
    // Update is called once per frame
    void Update()
    {

        //Debug.Log(Time.time + " // " + nextFire);
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextFire && canShoot)
        {
            nextFire = Time.time + 1f / fireRate;
            if (UseRayShoot)
            {
                GunAnim.Instance.anim.SetInteger("Fire", 2);
                Shoot_Ray();
            } else
            {
                Shoot_physics();
            }
            //currentRate = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            GunAnim.Instance.anim.SetInteger("Fire", -1);
        }
    }

    // Shoot using physics
    void Shoot_physics()
    {
        flashEffect.Play(true);
        //nextFire = Time.time + 1f / fireRate;
        GameObject project = Instantiate(projectile, shootPos.position, transform.localRotation);
        //project.GetComponent<ProjectileMover>().speed = projectileSpeed;
        project.GetComponent<EnemyHealth>().TakeDamage(projectileDamage);
        //var _rb = project.GetComponent<Rigidbody>();
        //_rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);
    }

    public Vector3 GetRayPoint()
    {
        
        RaycastHit hit;
        // first person
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, float.MaxValue, LayerMask.GetMask("Ground")))
        {

            // TODO hit enemy
            //Debug.Log(hit.transform.position);
            return hit.transform.position;
            
            //hit.transform.GetComponent<>();
        } else
        {
            return Vector3.zero;
        }

    }
    void Shoot_Ray()
    {

        MasterAudio.PlaySound3DAtTransform("Shoot", shootPos);
        recoil.RecoilFire();
        flashEffect.Play(true); // plays flash
        RaycastHit hit;
        // first person
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {

            // TODO hit enemy
            //Debug.Log(hit.transform.position);

            if (enableKnockBack)
            {
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(hit.normal * knockbackForce);
                }
            }


            EnemyHealth enemHealth = hit.transform.gameObject.GetComponent<EnemyHealth>();
            if (enemHealth != null)
            {  // if it hits the enemy
                enemHealth.TakeDamage(projectileDamage);
                crossHair.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                crossHair.color = Color.red;
            }

            GameObject imp = Instantiate(impactOnHit, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(imp, 1f);
            //hit.transform.GetComponent<>();
        }
    }

    void ReticleEffect()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.forward, Color.black);
            if (hit.collider.CompareTag("Enemy"))
            {
                //Debug.Log("JHitEnemy");
                //crossHair.color = Color.Lerp(crossHair.color, Color.cyan, Time.deltaTime * 2);
                //crossHair.transform.localScale = Vector3.Lerp(crossHair.transform.localScale, new Vector3(0.5f, 0.5f, 1), Time.deltaTime * 2);
            }
            else
            {
                //crossHair.color = Color.Lerp(crossHair.color, originalReticleColor, Time.deltaTime * 2);
                crossHair.transform.localScale = Vector3.Lerp(crossHair.transform.localScale, Vector3.one, Time.deltaTime * 2);
                crossHair.color = Color.white;
            }
        }
    }

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
