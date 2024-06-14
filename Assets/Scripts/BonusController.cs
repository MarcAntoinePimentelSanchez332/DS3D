using UnityEngine;

public class BonusController : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Exemple de bonus: ajouter une nouvelle balle
            GameManager.instance.LaunchNewBall();
            Destroy(gameObject); // Détruire le bonus après activation
        }
    }
}
