using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private float mySpeedX; 
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    private Rigidbody2D myBody;
    private Vector3 defaultLocalScale;
    public bool onGround;
    private bool canDoubleJump;
    [SerializeField] GameObject arrow;
    [SerializeField] bool attacking;  
    [SerializeField] float currentAttackTimer;
    [SerializeField] float defaultAttackTimer;
    private Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        attacking = false;
        myBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        defaultLocalScale = transform.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        #region yürüme
        mySpeedX = Input.GetAxis("Horizontal");
        myAnimator.SetFloat("Speed", Mathf.Abs(mySpeedX));
        myBody.velocity = new Vector2(mySpeedX*speed, myBody.velocity.y);
        #endregion

        #region player yüzünü dönmesi
        if (mySpeedX>0)
        {
            transform.localScale = new Vector3(defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);
        }
        else if (mySpeedX<0)
        {
            transform.localScale = new Vector3(-defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);
        }
        #endregion

        #region zıplama ve çift zıplama
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);
                myAnimator.SetTrigger("Jump");
                canDoubleJump = true;
            }
            else
            {
                if(canDoubleJump)
                {
                    myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);
                    myAnimator.SetTrigger("Jump");
                    canDoubleJump = false;
                }

            }
                
        }
        #endregion

        #region ok atma

        if (Input.GetMouseButtonDown(0))
        {
            if(attacking == false)
            {
                attacking = true;
                myAnimator.SetTrigger("Attack");
                Invoke("Fire", 0.5f);
            }
        }
        if (attacking == true)
        {
            currentAttackTimer -= Time.deltaTime;
        }
        else
        {
            currentAttackTimer = defaultAttackTimer;
        }
        if (currentAttackTimer <= 0)
        {
            attacking = false;
        }
        #endregion

        

    }
    void Fire()
    {
        GameObject okumuz = Instantiate(arrow, transform.position, Quaternion.identity);

        if (transform.localScale.x > 0)
        {
            okumuz.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0f);
        }
        else
        {
            okumuz.transform.localScale = new Vector3(-okumuz.transform.localScale.x, okumuz.transform.localScale.y, okumuz.transform.localScale.z);
            okumuz.GetComponent<Rigidbody2D>().velocity = new Vector2(-5f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Die();
        }
    }

    void Die()
    {
        myAnimator.SetFloat("Speed", 0);
        myAnimator.SetTrigger("Die");
        myBody.constraints = RigidbodyConstraints2D.FreezePosition;
        enabled = false;
    }
}
