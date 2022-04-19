using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class GunAnim : MonoBehaviour
{
    public static GunAnim Instance;

    public Animator anim;

    private void Awake()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayFoot()
    {
        MasterAudio.PlaySound3DAtVector3("footstep_dirt", transform.position - Vector3.down);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
