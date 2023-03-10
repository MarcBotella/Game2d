using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] GameObject m_slideDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    public Animator animatorCofre;
    public Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;

    public int attackDamage = 5;
    public float attackRadius = 1f;
    public float attackDelay = 1f;
    public LayerMask playerLayer;
    private float nextAttackTime;

    float movementButton = 0.0f;
    public TextMeshProUGUI TextMeshProUGUI; 
    public Image cartel;
    public Image textoJadis;
    bool hafinalizado = false;
    public AudioSource audioJump;
    public AudioSource audioSwort;
    public AudioSource audioKey;
    public AudioSource audioCofre;

    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        cartel.gameObject.SetActive(false);
        textoJadis.gameObject.SetActive(false);
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling )
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_animator.SetBool("WallSlide", m_isWallSliding);

        //Attack
        if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            audioSwort.Play();
            m_animator.SetTrigger("Attack" + m_currentAttack);
            
            
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position +  new Vector3(2f,2f,0), attackRadius, playerLayer);
            foreach (Collider2D player in hitPlayers)
            {
                player.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
            
              

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling && Mathf.Abs(m_body2d.velocity.y) < 0.01f)
        {
            m_grounded = false;
            audioJump.Play();
            m_animator.SetTrigger("Jump");
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }

        if(!hafinalizado){
            textoJadis.gameObject.SetActive(false);
        }else{
            textoJadis.gameObject.SetActive(true);
        }
        

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ZOOM")){
            GameObject.Find("MainVirtual").GetComponent<CinemachineVirtualCamera>().enabled = false;
        }

        if (other.CompareTag("NextLevel") && GameManager.key == 1){
            print("NextLevel");
            SceneManager.LoadScene("nextLevel");
        }

        if (other.gameObject.tag == "Cofre" && GameManager.key == 1)
        {
            animatorCofre.Play("Open");    
            animatorCofre.SetBool("Open",true);
            audioCofre.Play();
            GameManager.life = 100;
        }   

        if (other.CompareTag("key")){

            GameObject key = GameObject.Find("key");
            audioKey.Play();

            Destroy(key);
            GameManager.key = 1;

            TextMeshProUGUI.text = "1/1";
        }
        
        if (other.CompareTag("limit")){
            print("NextLevel");
            SceneManager.LoadScene("FirstLevel");
            GameManager.life = 100;
        }

        if (other.CompareTag("Cartel")){
            print("Cartel");
            cartel.gameObject.SetActive(true);
        }

         if (other.CompareTag("Castillo")){
            print("Castillo");
            SceneManager.LoadScene("BossFinal");
        }

        if (other.CompareTag("TextoJadis")){
            print("TextoJadis");
            hafinalizado = true;
            //textoJadis.gameObject.SetActive(true);
        }

        if (other.CompareTag("Final")){
           SceneManager.LoadScene("WinMenu");
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ZOOM")){
            GameObject.Find("MainVirtual").GetComponent<CinemachineVirtualCamera>().enabled = true;
        }
            
        
        if (other.CompareTag("Cartel")){
            print("Cartel");
            cartel.gameObject.SetActive(false);
        }

        if (other.CompareTag("TextoJadis")){
            print("TextoJadis");
            hafinalizado =false;
            //textoJadis.gameObject.SetActive(false);
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
    public void Jump()
    {
        if (m_grounded)
            m_body2d.AddForce(Vector2.up * m_jumpForce);
    }
    public void Move(float amount)
    {
        movementButton = amount;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(2f,2f,0), attackRadius);
    }
}
