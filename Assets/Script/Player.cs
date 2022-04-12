using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator myAnim;
    private bool isGrounded;
    [SerializeField] float speed = 15;
    [SerializeField] GameObject balaD;
    [SerializeField] GameObject balaI;
    [SerializeField] float fuerzaSalto = 12;
    private float proximoDisparo = 0f;
    [SerializeField] float cadenciaDisparo = 1;
    private new AudioSource audio;
    [SerializeField] AudioClip clip;
    private bool muerto = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent < Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        //StartCoroutine(MiCorrutina());

    }

    IEnumerator MiCorrutina()
    {
        while(true)
        {
            Debug.Log("Esperando 4 segundos");
            yield return new WaitForSeconds(4);
            Debug.Log("Pasaron 4 segundos");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dirH = Input.GetAxis("Horizontal");
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, 1.3f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, Vector2.down * 1.3f, Color.red);

        isGrounded = (ray.collider != null);
        //Debug.Log("colisionando con: "+ray.collider.gameObject.name);
        //este vetor new Vector2(0, -1)) = a Vector".down
        Jump();
        if (Input.GetKeyDown(KeyCode.F) && Time.time > proximoDisparo)
        {
            SonidoShot();
            StartCoroutine("Fire");
            if (this.transform.localScale.x == 1)
            {
                proximoDisparo = Time.time + cadenciaDisparo;
                Instantiate(balaD, transform.position, transform.rotation);
            }
            if (this.transform.localScale.x == -1)
            {
                proximoDisparo = Time.time + cadenciaDisparo;
                Instantiate(balaI, transform.position, transform.rotation);
            }

        }
    }
    IEnumerator Fire()
    {
        myAnim.SetLayerWeight(1, 1);
        yield return new WaitForSeconds(0.8f);
        myAnim.SetLayerWeight(1, 0);
    }
    void Jump()
    {
        if(isGrounded)
        {   
            if(Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
                //impulse añade una fuerza instantanea al rb usando la masa, miestras en force no es instantaneo, es decir se demora un poquita.
                Debug.Log(isGrounded);
            }
        }
        if(rb.velocity.y != 0 && !isGrounded)
        {
            myAnim.SetBool("isJumping", true);
        }
        else
        {
            myAnim.SetBool("isJumping", false);
        }
    }
    void FinishingRun()
    {
        Debug.Log("Termina animacion de correr");
    }
    private void FixedUpdate()
    {
        if(muerto == false)
        {
            float dirH = Input.GetAxis("Horizontal");
            if (dirH != 0)
            {
                myAnim.SetBool("isRunning", true);
                if (dirH < 0)
                {
                    transform.localScale = new Vector2(-1, 1);
                }
                else
                {
                    transform.localScale = new Vector2(1, 1);
                }
            }
            else
            {
                myAnim.SetBool("isRunning", false);
            }
            rb.velocity = new Vector2(dirH * speed, rb.velocity.y);
        }
        else
        {
            gameObject.layer = 6;
            rb.constraints = (RigidbodyConstraints2D)RigidbodyConstraints.FreezePosition;
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Debug.Log(collision.gameObject.layer);
            StartCoroutine("restart");
        }
        if (collision.gameObject.layer == 10)
        {
            Debug.Log(collision.gameObject.layer);
            StartCoroutine("restart");
        }
    }
    IEnumerator restart()
    {
        myAnim.SetBool("isDead", true);
        muerto = true;
        yield return new WaitForSeconds(1);
        SonidoDead();
        yield return new WaitForSeconds(1);
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
    public void SonidoShot()
    {
        audio.Play();
    }
    public void SonidoDead()
    {
        audio.PlayOneShot(clip);
    }
}
        //2dphysics.overlap.circle es una para detectar con que colisiona, pero vamos a utilizar physics.raycast(origen, dir, distancia) es una linea perpendicular, perfecta para saber si colisiona con el piso
        // el raycast es mas barato(me nos costosa para unity) que el overlap circle
