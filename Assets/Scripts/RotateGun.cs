using Photon.Pun;
using UnityEngine;

public class RotateGun : MonoBehaviour
{


    public float rotSpeed = 5f;
    private PhotonView pv;
    public PlayerController player;


    //public Camera mainCamera;

    void Awake()
    {
        pv = gameObject.GetComponent<PhotonView>();
        player = gameObject.GetComponentInParent<PlayerController>();
    }
    void Update()
    {
        RotateGunToMouse(player.facingRigth);
    }

    void RotateGunToMouse(bool facingRight = true)
    {
        if (pv.IsMine)
        {
            //Vector3 mouse = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = mouse - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg ;

            angle = facingRight ? angle : 180f + angle;
            //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }
}
