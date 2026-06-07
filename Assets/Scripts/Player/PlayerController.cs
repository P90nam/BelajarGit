using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerData data;

    private float currentHP;
    private float speed;

    private PlayerInput playerInput;
    private Vector2 moveInput;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    private float attackInput;
    private float previousAttackInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        if (data != null)
        {
            currentHP = data.maxHP;
            speed = data.moveSpeed;
        }
        else
        {
            Debug.LogError("Nan, file PlayerData belum kamu masukin ke slot 'Data' di Inspector Player!");
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentState != GameState.Playing)
        {
            return; 
        }

        if (playerInput == null) return;

        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        attackInput = playerInput.actions["Attack"].IsPressed() ? 1f : 0f;

        float h = moveInput.x;
        float v = moveInput.y;

        transform.Translate(new Vector3(h, v, 0) * speed * Time.deltaTime);
        
        if (previousAttackInput == 0 && attackInput > 0)
        {
            Shoot();
        }
        previousAttackInput = attackInput;
    }

    void Shoot()
    {
        Debug.Log("Player is shooting!");

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0;

        Vector3 spawnPos = bulletSpawnPoint != null ? bulletSpawnPoint.position : transform.position;
        Vector3 shootDirection = (mouseWorldPos - spawnPos).normalized;

        GameObject bulletObj = PooledObjects.Instance.GetPooledObject();

        if (bulletObj != null)
        {
            bulletObj.transform.position = spawnPos + shootDirection * 0.3f;
            bulletObj.transform.rotation = Quaternion.identity;
            
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetDirection(shootDirection); // set direction SEBELUM SetActive
            }
            
            bulletObj.SetActive(true); // aktifkan SETELAH SetDirection
        }
        else
        {
            Debug.LogWarning("No pooled bullet available!");
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            TakeDamage(0.1f);
        }
    }

    void TakeDamage(float dmg)
    {
        currentHP -= dmg;
        Debug.Log("Player HP: " + currentHP);

        if (currentHP <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }
}