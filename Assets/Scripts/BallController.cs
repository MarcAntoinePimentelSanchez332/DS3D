using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 30f;
    public float angleVariance = 15f;
    private Rigidbody rb;
    private Vector3 direction;
    private Transform paddleTransform;

    private bool isLaunched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the ball.");
            return;
        }
        rb.velocity = Vector3.zero;
    }

    void Update()
    {
        if (!isLaunched && paddleTransform != null)
        {
            // Suivre la position de la barre
            Vector3 newPosition = paddleTransform.position;
            newPosition.y = transform.position.y; // Garder la même hauteur pour la balle
            newPosition.z += 1.0f; // Ajuster la position de la balle par rapport à la barre
            transform.position = newPosition;

            // Lancer la balle seulement si le jeu a commencé et que la balle n'est pas encore lancée
            if (GameManager.instance.gameStarted && Input.GetKeyDown(KeyCode.Space))
            {
                LaunchBall();
            }
        }
    }

    void FixedUpdate()
    {
        // Maintenir la vitesse de la balle constante pendant que le jeu est en cours
        if (GameManager.instance.gameStarted && isLaunched && rb != null)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
    }

    public void LaunchBall()
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the ball.");
            return;
        }

        // Détermine un angle aléatoire dans un cône de ±30 degrés autour de l'axe Z négatif
        float angle = Random.Range(-30f, 30f);
        float angleRad = angle * Mathf.Deg2Rad;

        // Calculer la direction avec l'angle par rapport à l'axe Z négatif
        direction = new Vector3(Mathf.Sin(angleRad), 0, -Mathf.Cos(angleRad));

        // Appliquer la vitesse à la direction
        rb.velocity = direction * speed;
        isLaunched = true;
    }

    public void SetPaddleTransform(Transform paddle)
    {
        paddleTransform = paddle;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision == null || collision.gameObject == null)
        {
            return;
        }

        Debug.Log("Collision with: " + collision.gameObject.name);

        // Vérifier si la balle touche une brique
        Brick brick = collision.gameObject.GetComponent<Brick>();
        if (brick != null)
        {
            brick.Hit();
            GameManager.instance.AddScore(10);
        }

        // Calculer la réflexion de la balle avec une variance d'angle aléatoire
        Vector3 normal = collision.contacts[0].normal;
        Vector3 reflectDir = Vector3.Reflect(rb.velocity, normal);

        float angle = Random.Range(-angleVariance, angleVariance);
        reflectDir = Quaternion.Euler(0, angle, 0) * reflectDir;

        rb.velocity = reflectDir.normalized * speed;
    }
}
