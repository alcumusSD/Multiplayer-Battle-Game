using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    Animator ani;
    public bool hasAttacked = false;
    Rigidbody2D rb;
    float moveX;
    float moveY;
    GameObject enemy;
    //Vector2 moveY;
    bool onFloor = true;
    public Image healthBar;
    public float healthAmount = 100f;
    Animator aniEnemy;
    public Image border;
    public Image red;
    PhotonView view;
    TMP_Text username;


    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //aniEnemy = PhotonNetwork.PlayerList[1].GetComponent<Animator>();
        view = GetComponent<PhotonView>();
        username = GetComponentInChildren<TMP_Text>();
        username.text = view.Controller.NickName;
        //moveY = new Vector2();
        float move;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //collision.collider.gameObject.tag == ("Sword")
        Animator aniEnemy = collision.gameObject.GetComponent<Animator>();
        Debug.Log("OnCollisionEnter2D");
        if (aniEnemy.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            Debug.Log("hit");
            TakeDamage(10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            Debug.Log("view.IsMine");
            Debug.Log("healthAmount" + healthAmount);
            
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");

            if (moveX != 0 || moveY != 0)
            {
                ani.SetBool("walk", true);
            }
            else
            {
                ani.SetBool("walk", false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                ani.SetBool("attack", true);
                hasAttacked = true;
            }
            else
            {
                ani.SetBool("attack", false);
            }
        }

        view.RPC("SyncMove", RpcTarget.All, moveX);
    }

    private void FixedUpdate()
	{
        //rb.AddForce(move);
        rb.velocity = new Vector2(moveX * 20, moveY*20);
    }

    public void TakeDamage(float damage)
    {         
        Debug.Log("dmg");
        healthAmount -= damage;
        view.RPC("SyncHealth", RpcTarget.All, healthAmount);
    }

    [PunRPC]
    private void SyncHealth(float newHealth)
    {
        // Update the health variable on all clients
        healthBar.fillAmount = newHealth / 100f;

        // Optionally, update the health bar UI or perform other actions based on the new health value
        if (newHealth <= 0)
        {
            Destroy(healthBar);
            Destroy(border);
            Destroy(red);
            Crumble(transform);
        }
    }

    [PunRPC]
    private void SyncMove(float newMove)
    {
        // Update the move on all clients
        if (newMove > 0)
        {
            transform.localScale = new Vector3(-3, 3, 3);
            healthBar.transform.localScale = new Vector3(1, 1, 1);
            username.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (newMove < 0)
        {
            transform.localScale = new Vector3(3, 3, 3);
            healthBar.transform.localScale = new Vector3(-1, 1, 1);
            username.transform.localScale = new Vector3(1, 1, 1);
        }

        // Optionally, update the health bar UI or perform other actions based on the new health value
    }

    void Crumble(Transform t)
    {       
        Transform parent = transform.parent;

        foreach (Transform child in t.GetComponentInChildren<Transform>())
        {
            Crumble(child);
        }

        Debug.Log("Print Crumbling");
        Rigidbody2D body = t.gameObject.AddComponent<Rigidbody2D>();
        body.gravityScale = 9;
        float directionX = Random.Range(-5, 50);
        float directionY = Random.Range(5, 50);
        float torque = Random.Range(5, 8);
        body.AddForce(new Vector2(directionX,directionY), ForceMode2D.Impulse);
        Debug.Log("Bro");
        Destroy(t.gameObject, 1);
        ani.enabled = false;
    }
}