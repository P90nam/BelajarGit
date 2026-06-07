using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;

    void OnEnable()
    {
        // HAPUS reset direction di sini
        StopAllCoroutines();
        StartCoroutine(DeactivateRoutine());
    }

    IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (direction != Vector3.zero)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;
        if (collision.gameObject.layer == gameObject.layer) return;
        gameObject.SetActive(false);
    }
}