using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float playerSpeed;
    public float sprintSpeed;
    private float moveSpeed;
    [Header("Jump Settings")]
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    [Header("Health Bar")]
    public float health  = 20f;
    public Slider helthBar;
    public Image image_bar;
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode SprintSpeed = KeyCode.LeftShift;
    public KeyCode Pistol = KeyCode.F;
    public KeyCode Shotgun = KeyCode.G;
    public KeyCode Ak_47 = KeyCode.H;
    public KeyCode Pauce = KeyCode.Escape;
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public Transform orientation;
    private bool _grounded;
    [Header("Gun Style")]
    public GunStyle currentGun;
    public GameObject[] Gun;
    public GameObject[] Inventory;
    [Header("Pauce Menu / Die")]
    public GameObject pauce;
    public GameObject GameOver;
    public GameObject WinGame;
    bool CanAcrivePause;
    [Header("Enemyes")]
    public TextMeshProUGUI enemyesValue;
    public static int enemyesCount = 0;
    public int enemyOnScene;
    public GameObject theEnemy;
    private float xPos;
    private float zPos;
    public static int enemyCount;
    public int howMuch;
    [Header("Win Count")]
    public TextMeshProUGUI Win_text;
    public TextMeshProUGUI Die_text;
    public static int winCount = 0;
    public static int dieCount = 0;

    private Vector3 spawnPoint = new Vector3(-2.1f, 1.1f, 28f);
    private Rigidbody rb;
    private Vector3 moveDirection;
    private float _horizontalInput;
    private float _verticalInput;
    
   
    
    public enum GunStyle
    {
        Pistol,
        Shotgun,
        Ak_47

    }
    private void Start()
    {
        
        enemyCount = 0;
        gameObject.SetActive(true);
        StartCoroutine(enemyDrop());
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        transform.position = spawnPoint;
        if (!PlayerPrefs.HasKey("Win"))
        {
            PlayerPrefs.SetInt("Win", 0);
        }
        if (!PlayerPrefs.HasKey("Die"))
        {
            PlayerPrefs.SetInt("Die", 0);
        }

    }

    private void Update()
    {
        Text();
        MyInput();
        SpeedControl();
        WinLevel();
       



        if (WinGame.activeInHierarchy)
        {
            CanAcrivePause = false;
        }
        else
        {
            CanAcrivePause = true;
        }

       
        //Input
        if (Input.GetKeyDown(Pistol)) SwitchGunStyle(GunStyle.Pistol);
       
        if (Input.GetKeyDown(Shotgun)) SwitchGunStyle(GunStyle.Shotgun);
       
        if (Input.GetKeyDown(Ak_47)) SwitchGunStyle(GunStyle.Ak_47);

        if (Input.GetKeyDown(Pauce) && CanAcrivePause == true) Toggle();

        //HealthBar        
        helthBar.value = health;
        if(health < 2) image_bar.enabled = false;
        else image_bar.enabled = true;

         // ground check
        _grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
                
        // handle drag
        if (_grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }
    private void Text()
    {
         dieCount = PlayerPrefs.GetInt("Die");
         winCount = PlayerPrefs.GetInt("Win");
        
        enemyesValue.SetText(enemyesCount + "/" + enemyOnScene);
        Win_text.SetText("You win: " + winCount);
        Die_text.SetText("Enemy win: " + dieCount);
        
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    

    private void MyInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        // when to sprint
        if (Input.GetKey(SprintSpeed) && _grounded)   
        moveSpeed = sprintSpeed;
        else 
        moveSpeed = playerSpeed;

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && _grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        // on ground
        if (_grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!_grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
       // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

    }
    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
    private void sprint_Speed()
    {
        Vector3 SprintVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (SprintVel.magnitude > sprintSpeed)
        {
            Vector3 SprintedVel = SprintVel.normalized * sprintSpeed;
            rb.velocity = new Vector3(SprintedVel.x, rb.velocity.y, SprintedVel.z);
        }


    }
    private void SwitchGunStyle(GunStyle newStyle)
    {
        Gun[0].SetActive(false);
        Gun[1].SetActive(false);
        Gun[2].SetActive(false);

        Inventory[0].SetActive(false);
        Inventory[1].SetActive(false);
        Inventory[2].SetActive(false);

        if (newStyle == GunStyle.Pistol)
        {   
            Gun[0].SetActive(true); 
            Inventory[0].SetActive(true); 
        } 
        if (newStyle == GunStyle.Shotgun)
        {   
            Gun[1].SetActive(true); 
            Inventory[1].SetActive(true); 
        } 
        if (newStyle == GunStyle.Ak_47)
        {
            Gun[2].SetActive(true);
            Inventory[2].SetActive(true);
        }
        currentGun = newStyle;
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Done");
        if (health <= 0)
        {
         Game_Over();

        }
    }
      
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Destroy")
        {
            Game_Over();
            
        }
    }
    public void Game_Over()
    {
        CanAcrivePause = false;
        GameOver.SetActive(true);
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0f;
        enemyesCount = 0;
        dieCount++;
        PlayerPrefs.SetInt("Die", dieCount);
       

    }
    public void WinLevel()
    {
        if (enemyesCount == enemyOnScene)
        {
            CanAcrivePause = false;
            WinGame.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            
        }
        
    }
    public void nextLevel()
    {
            winCount++;
            PlayerPrefs.SetInt("Win", winCount);
            enemyCount = 0;
            StartCoroutine(enemyDrop());
            WinGame.SetActive(false);
            Time.timeScale = 1f;
            enemyesCount = 0;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            gameObject.SetActive(true);

    }
    public void Toggle()
    {
        pauce.SetActive(!pauce.activeSelf);
        if (pauce.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;

        }
    }
   
    public void Retry()
    {
        enemyCount = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        enemyesCount = 0;
        gameObject.SetActive(true);
        StartCoroutine(enemyDrop());
       
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    IEnumerator enemyDrop()
    {
        while (enemyCount < howMuch)
        {
            xPos = Random.Range(26.2f, -27.8f);
            zPos = Random.Range(-1.5f, -32.8f);
            Instantiate(theEnemy, new Vector3(xPos, 1.43f, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }
    
   

}