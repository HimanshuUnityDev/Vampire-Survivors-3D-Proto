using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Vector3 moveDirection;
    private float moveSpeed;
    private float damage;

    public void Init(Vector3 direction, float damage, float speed, float lifetime)
    {
        this.moveDirection = direction.normalized;
        this.damage = damage;
        this.moveSpeed = speed;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
