using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemyKilled;
    public AudioClip enemyFXS;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            DestroyEnemy();
        }
    }

    void DestroyEnemy()
    {
        AudioSource.PlayClipAtPoint(enemyFXS, transform.position);
        Instantiate(enemyKilled, transform.position, transform.rotation);
        gameObject.SetActive(false);
        Destroy(gameObject, 0.5f);
    }
}
