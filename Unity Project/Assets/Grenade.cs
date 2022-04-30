using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class Grenade : MonoBehaviour
{
    [SerializeField]
    private Transform shootPos;

    [SerializeField]
    private GameObject grenade;

    [SerializeField]
    private GameObject grenadeVisual;

    Animator anim;
    [SerializeField]
    bool isGrenade;
    public float throwForce;

    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        grenade.SetActive(false);
        

    }
    public void ShowGrenade()
    {
        grenadeVisual.SetActive(true);
    }
    public void HideGrenade()
    {
        grenadeVisual.SetActive(false);
    }

    public void EnableGrenade()
    {
        isGrenade = false;
    }

    public void PlayGrenadePull()
    {
        MasterAudio.PlaySound3DAtVector3("Grenade_pull", transform.position);
    }
    
    public void ThrowGrenade()
    {
        GameObject gren = Instantiate(grenade, shootPos.position, shootPos.rotation);
        gren.SetActive(true);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(shootPos.forward * throwForce, ForceMode.Impulse);

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && !isGrenade)
        {
            anim.SetTrigger("Grenade");
            isGrenade = true;
        }
    }
}
