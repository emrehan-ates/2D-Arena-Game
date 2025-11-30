using UnityEngine;
using Photon.Pun;

public class PlayerSync : MonoBehaviourPun, IPunObservable
{
    Vector3 networkPos;
    Quaternion networkRot;
    Vector2 networkVelocity;
    private float lag;
    [SerializeField] public Rigidbody2D rb;

    void Start()
    {
        if (!photonView.IsMine)
        {
            rb.isKinematic = true;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) 
        {
            stream.SendNext(transform.position);
            stream.SendNext(rb.linearVelocity);
            //stream.SendNext(transform.rotation);
        }
        else
        {
            networkPos = (Vector3)stream.ReceiveNext();
            networkVelocity = (Vector2)stream.ReceiveNext();
            //networkRot = (Quaternion)stream.ReceiveNext();
            
            lag = (float)(PhotonNetwork.Time - info.SentServerTime);
            networkPos += new Vector3(networkVelocity.x, networkVelocity.y, 0) * lag;
        }
    }

    //fixed olmadan da dene
    void Update()
    {
        if (!photonView.IsMine)
        {
            
            rb.MovePosition(Vector3.Lerp(transform.position, networkPos, Time.deltaTime * 10f));
            //transform.position = Vector3.Lerp(transform.position, networkPos, Time.deltaTime * 10);
            //transform.rotation = Quaternion.Lerp(transform.rotation, networkRot, Mathf.Clamp01( Time.deltaTime * 10));
        }
    }
}
