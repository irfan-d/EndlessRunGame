using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;       //Kecepatan player
    public float laneSpeed;
    public bool isJumping = false;
    public bool comingDown = false;
    public bool isSliding = false;
    public bool comingUp = false;
    public int maxLife = 3;
    public float minSpeed = 7f;
    public float maxSpeed = 15f;
    public float invincibleTime;

    public GameObject playerObject;
    public Rigidbody rb;
   
    private int currentLife;
    private UIManager uiManager;
    
    private int currentLane = 1;
    private BoxCollider boxCollider;
    private Vector3 boxColliderSize;
    private Vector3 verticalTargetPosition;
    private LifeDisplay LifeDisplay;
    private Animator anim;
    private bool invincible = false;
    static int blinkingValue;
    private float score;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //anim = GetComponent<Animator>();
        anim = GetComponentInChildren<Animator>();

        boxCollider = GetComponent<BoxCollider>();
        boxColliderSize = boxCollider.size;

        currentLife = maxLife;
        uiManager = FindAnyObjectByType<UIManager>();

        speed = minSpeed;

        blinkingValue = Shader.PropertyToID("_BlinkingValue");
        uiManager.UpdateLives(currentLife); 
    }

    void Update()
    {
        score += Time.deltaTime * speed;
        uiManager.UpdateScore((int)score);


        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))          //------------------ Jika menekan tombol A atau arrow kiri
        {
            ChangeLane(-1);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) )    //---------------- Jika menekan tombol D atau arrow kanan
        {
            ChangeLane(1);
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))    //---------------- Jika menekan tombol D atau arrow kanan
        {
            if (isJumping == false)
            {
                isJumping=true;
                playerObject.GetComponent<Animator>().Play("Jump");
                StartCoroutine(JumpSequence());
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!isSliding) // Cek jika karakter tidak sedang slide
            {
                isSliding = true;
                playerObject.GetComponent<Animator>().Play("Slide");
                StartCoroutine(SlideSequence());
            }

        }


        if (isJumping == true)
        {
            if (comingDown == false)
            {
                transform.Translate(Vector3.up * Time.deltaTime * 4, Space.World);
            }
            if (comingDown == true)
            {
                transform.Translate(Vector3.up * Time.deltaTime * -4, Space.World);
            }

        }

        if (isSliding)
        {
            if (comingUp == false)
            {
                transform.Translate(Vector3.down * Time.deltaTime * 3, Space.World);
            }
            if (comingUp == true)
            {
                transform.Translate(Vector3.up * Time.deltaTime * 3, Space.World);
            }
        }


        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, rb.position.y, rb.position.z);
        rb.MovePosition(Vector3.MoveTowards(rb.position, targetPosition, laneSpeed * Time.deltaTime));
    }

    IEnumerator JumpSequence()
    {
        yield return new WaitForSeconds(0.45f);
        comingDown =true;
        yield return new WaitForSeconds(0.45f);
        isJumping = false;
        comingDown=false;
        playerObject.GetComponent<Animator>().Play("Running");

    }

    IEnumerator SlideSequence()
    {
        // Kecilkan box collider agar sesuai posisi "slide"
        boxCollider.size = new Vector3(boxColliderSize.x, boxColliderSize.y / 2, boxColliderSize.z);
        yield return new WaitForSeconds(0.3f); // Tunggu selama 0.3 detik, karakter turun
        comingUp = true; // Mulai naik
        yield return new WaitForSeconds(0.55f); // Tunggu selama 0.3 detik, karakter kembali ke posisi semula
        isSliding = false;
        comingUp = false;
        // Kembalikan collider ke ukuran aslinya
        boxCollider.size = boxColliderSize;
        // Ganti animasi ke "Running"
        playerObject.GetComponent<Animator>().Play("Running");
    }

    private void FixedUpdate()
    {
        //rb.velocity = Vector3.forward * speed;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, speed);

    }

    void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;
        if (targetLane < 0 || targetLane > 2)
            return;

        currentLane = targetLane;
        //verticalTargetPosition = new Vector3((currentLane - 1) * 3, 0, 0);
        verticalTargetPosition = new Vector3((currentLane - 1) * 3, verticalTargetPosition.y, 0); // Pindah ke jalur target
    }

    private void OnTriggerEnter(Collider other)
    {
        if (invincible)
            return;
        if (other.CompareTag("Obstacle")) // Jika karakter menyentuh rintangan dan tidak invincible
        {
            Debug.Log("Karakter terkena rintangan!");

            // Kurangi nyawa
            currentLife--;
            uiManager.UpdateLives(currentLife); // Update tampilan nyawa
            speed = 0;

            if (currentLife <= 0)   
            {
                speed = 0;
                anim.Play("Stumble Backwards"); // Mainkan animasi Stumblebackwards
                uiManager.gameOverPanel.SetActive(true);
                Invoke("CallMenu", 2f);
            }
            else
            {
                StartCoroutine(Blinking(invincibleTime));
            }
        }

    }

    IEnumerator Blinking(float time)
    {
        invincible = true;
        float timer = 0;
        float currentBlink = 1f;
        float lastBlink = 0f;
        float blinkPeriod = 0.1f;
        bool enabled = false;
        yield return new WaitForSeconds(1f);
        speed = minSpeed; 
        while( timer < time && invincible )
        {
            playerObject.SetActive(enabled);
            //Shader.SetGlobalFloat(blinkingValue, currentBlink);
            yield return null;
            timer += Time.deltaTime;
            lastBlink += Time.deltaTime;
            if(blinkPeriod < lastBlink)
            {
                lastBlink = 0;
                currentBlink = 1f - currentBlink;
                enabled = !enabled;
            }
        }
        playerObject.SetActive(true);
        //Shader.SetGlobalFloat(blinkingValue, 0);
        invincible = false;
    }
    void CallMenu()
    {
        GameManager.gm.EndRun();
    }

    //public void IncreaseSpeed()
    //{
    //    speed *= 1.15f;
    //    if (speed >= maxSpeed)
    //        speed = maxSpeed;
    //}
}




