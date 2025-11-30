using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPun
{
    public float bulletForce;
    private Vector2 direction;
    private Rigidbody2D rb;
    public AudioClip clip;
    public AudioSource source;
    
    public void Init(Vector2 dir, float force)
    {
        direction = dir;
        bulletForce = force;
    }

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletForce;

        if (!photonView.IsMine)
        {
            rb.isKinematic = true;
        }
        source = GetComponent<AudioSource>();
    }

    [PunRPC]
    public void InitBullet(Vector2 dir, float force)
    {
        direction = dir;
        bulletForce = force;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (photonView.IsMine)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.linearVelocity = dir * force * 5;
        }
        source.PlayOneShot(clip);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (!photonView.IsMine)
            return;
        
        
        GameObject enemy = collision.gameObject;

        if (enemy.CompareTag("Player") )
        {
            enemy.GetComponent<PhotonView>().RPC("TakeDamage", enemy.GetComponent<PhotonView>().Owner , 8);
            photonView.RPC("DestroyBullet", RpcTarget.All);
        }
        else
        {
            photonView.RPC("DestroyBullet", RpcTarget.All);
        }
    }

    private void Update()
    {
        if(!photonView.IsMine) return;
        if (transform.position.y < -6 || transform.position.y > 6)
        {
            photonView.RPC("DestroyBullet", RpcTarget.All);
            Debug.Log("out of bounds y");
        }

        if (transform.position.x < -10 || transform.position.x > 10)
        {
            photonView.RPC("DestroyBullet", RpcTarget.All);
            Debug.Log("out of bounds x");
        }
    }
    
    [PunRPC]
    void DestroyBullet()
    {
        if (photonView.IsMine) 
        {
            Destroy(gameObject);
        }
    }
}

