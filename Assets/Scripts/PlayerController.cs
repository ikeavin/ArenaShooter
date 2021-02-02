using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float maxHealth;
    public float health;
    private float speed;
    private float verticalInput;
    private float horizontalInput;
    public Origin origin = Origin.Player;

    // Start is called before the first frame update
    void Start()
    {
        health = 5;
        maxHealth = 5;
        speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * speed);
        transform.Translate(Vector3.forward * Time.deltaTime * verticalInput * speed);

        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            Vector3 direction = hit.point;
            direction -= transform.position;
            direction = direction.normalized;
            direction.y = transform.position.y;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().SetDirection(direction);
            bullet.GetComponent<Bullet>().SetOrigin(this.origin);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();

        if (bullet != null && bullet.GetOrigin() != this.origin)
        {

            this.health -= bullet.GetDamage();
            Destroy(collision.gameObject);

            if(this.health <= 0)
            {
                Time.timeScale = 0f;
            }
        }
    }
}
