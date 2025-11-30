using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Health : MonoBehaviourPun, IPunObservable
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    
    public Slider healthBar;
    public PlayerController player;

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        //healthBar = gameObject.GetComponentInChildren<Slider>();
        player = gameObject.GetComponent<PlayerController>();
    }
    
    [PunRPC]
    public void TakeDamage(int amount)
    {

        //if (!photonView.IsMine) return;
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthBar();

        if (currentHealth == 0)
        {
            Die();
        }
    }

    
    void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
        else
            Debug.LogError("HealthBar is null");
    }
    
    
    
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth); 
        }
        else
        {
            currentHealth = (int)stream.ReceiveNext(); 
            UpdateHealthBar();
        }
    }
    void Die()
    {
        Debug.Log($"{photonView.Owner.NickName} dies!");
        player.isGameOver = true;
    }
}
