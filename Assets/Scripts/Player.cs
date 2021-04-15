using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : NetworkBehaviour
{
    public int maxHealth = 1;
    public float speed = 1f;
    public float thrust = 2f;
    public float bulletSpeed = 10f;
    public float shootTime = 2f;
    public GameObject bullet;
    public Transform endOfBarrel;
    public Transform groundPoint;
    public LayerMask whatIsGround;

    private bool isGrounded = true;
    private int currentHealth;
    private int score;
    private float currentShootTime;
    private Vector3 spawnpoint;
    private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;

    [Client]
    private void Start()
    {
        if (this.isLocalPlayer)
        {
            this.gameObject.name = "BigPP";
            this.currentHealth = this.maxHealth;
            this.score = 0;
            this.currentShootTime = this.shootTime;
            this.rigidbody = this.GetComponent<Rigidbody2D>();
            this.animator = this.GetComponent<Animator>();
            this.spawnpoint = this.transform.position;
        }
    }

    private void Update()
    {
        if (this.gameObject.name != "BigPP")
        {
            
        }
        if (this.isLocalPlayer)
        {
            this.HandleMovement();
            this.CheckGrounded();
            this.HandleJump();
            this.HandleShooting();
            this.HandleAnimations();
            this.HandleCooldowns();

            // for debugging
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(HandlePlayerDeath(1.5f));
            }
        }
    }

    private void HandleMovement()
    {
        float x_input = 0f;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            x_input = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            x_input = -1;
        }

        this.rigidbody.velocity = new Vector2(x_input * this.speed, this.rigidbody.velocity.y);

        this.HandleRotation(x_input);
    }

    private void HandleRotation(float direction)
    {
        if (direction < 0)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else if (direction > 0)
        {
            this.transform.rotation = Quaternion.identity;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Z) && this.isGrounded)
        {
            this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.x, this.thrust);
        }
    }

    private void CheckGrounded()
    {
        this.isGrounded = Physics2D.OverlapCircle(this.groundPoint.position, 0.2f, this.whatIsGround);
    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.X) && this.currentShootTime <= 0)
        {
            this.CmdShoot();

            this.currentShootTime = this.shootTime;
        }
    }

    private void HandleAnimations()
    {
        bool is_moving = this.rigidbody.velocity.x != 0;
        bool is_jumping = this.rigidbody.velocity.y != 0;

        this.animator.SetBool("is_moving", is_moving);
        this.animator.SetBool("is_jumping", is_jumping);
        this.animator.SetFloat("y_velocity", this.rigidbody.velocity.y);
        this.animator.SetBool("is_damaged", false);
    }

    private void HandleCooldowns()
    {
        this.currentShootTime -= Time.deltaTime;
    }

    private IEnumerator HandlePlayerDeath(float time)
    {
        this.score++;
        this.animator.SetBool("is_damaged", true);

        yield return new WaitForSeconds(time);

        //this.CmdDestroyPlayer();
        this.currentHealth = this.maxHealth;
        this.transform.position = this.spawnpoint;
        GameObject.Find("Score").GetComponent<Text>().text = this.score.ToString();
    }

    [Command]
    private void CmdShoot()
    {
        GameObject clone = Instantiate(this.bullet, this.endOfBarrel.position, Quaternion.identity);

        clone.GetComponent<Rigidbody2D>().velocity = Vector2.left * this.bulletSpeed * (this.transform.rotation.y.Equals(0) ? -1 : 1);

        NetworkServer.Spawn(clone, this.gameObject);

        Destroy(clone, 2f);
    }

    [Command]
    private void CmdDestroyPlayer()
    {
        NetworkServer.Destroy(this.gameObject);
    }

    public void TakeDamage(int damage)
    {
        this.currentHealth -= damage;

        if (this.currentHealth <= 0 && this.isLocalPlayer)
        {
            StartCoroutine(HandlePlayerDeath(1));
        }
    }
}
