//using System.Numerics;

using System.Collections;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public Rigidbody2D rb;
    public float jump = 10f;
    public float forward = 5f;
    private bool onGround = false;
    UnityEngine.Vector3 rigthWay = new UnityEngine.Vector3(1, 0, 0);
    UnityEngine.Vector3 leftWay = new UnityEngine.Vector3(-1, 0, 0);
    UnityEngine.Vector3 upWay = new UnityEngine.Vector3(0, 1, 0);
    private CapsuleCollider2D cd;
    public bool isGameOver = false;
    public GameObject parent;
    public bool facingRigth = true;
    public Animator anim;
    private PhotonView pv;
    public bool isWalking;
    public bool isJumping;
    public GameObject bulletPrefab;
    bool done = false;
    public Transform firepoint;
    float bulletForce = 20f;
    public string team;
    [SerializeField] Canvas canvas;

    void Awake()
    {
        if (!photonView.IsMine)
        {
            rb.isKinematic = true;
        }
        //rb = gameObject.GetComponent<Rigidbody2D>();
        cd = gameObject.GetComponent<CapsuleCollider2D>();
        pv = gameObject.GetComponent<PhotonView>();
        anim = gameObject.GetComponent<Animator>();
        canvas = FindFirstObjectByType<Canvas>();
        //canvas = gameObject.GetComponent<Canvas>();
        Time.timeScale = 1;
    }

    void FixedUpdate()
    {


        if (isGameOver && pv.IsMine)
        {
            rb.linearVelocity = UnityEngine.Vector2.zero;
            rb.angularVelocity = 0f;
            //StartCoroutine(WaitForCanvas());
        }

        else if (pv.IsMine)
        {
            HandleInput();
        }

        if (transform.position.y < -14)
        {
            isGameOver = true;
        }



    }
    

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(leftWay * forward * Time.deltaTime, ForceMode2D.Force);
            anim.SetBool("isWalking", true);
            isWalking = true;
            if (facingRigth)
            {
                Flip();
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(rigthWay * forward * Time.deltaTime, ForceMode2D.Force);
            anim.SetBool("isWalking", true);
            isWalking = true;
            if (!facingRigth)
            {
                Flip();
            }
        }
        else
        {
            anim.SetBool("isWalking", false);
            isWalking = false;
        }


        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && onGround)
        {
            rb.AddForce(upWay * jump * Time.deltaTime, ForceMode2D.Impulse);
            anim.SetTrigger("isJumping");
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    void Flip()
    {
        UnityEngine.Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        facingRigth = !facingRigth;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            //anim.SetBool("isJumping", false); its trigger now
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }




    void Shoot()
    {

        // GameObject bul = PhotonNetwork.Instantiate(bulletPrefab.name, firepoint.position, firepoint.rotation);
        // Rigidbody2D brb = bul.GetComponent<Rigidbody2D>();
        // brb.AddForce(firepoint.right * bulletForce, ForceMode2D.Impulse);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = (mousePos - firepoint.position).normalized;

        GameObject bul = PhotonNetwork.Instantiate(bulletPrefab.name, firepoint.position, Quaternion.identity);
        bul.GetComponent<PhotonView>().RPC("InitBullet", RpcTarget.All, shootDir, bulletForce);
    }
    


  

}
