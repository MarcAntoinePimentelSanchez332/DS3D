using UnityEngine;

public class BottomBarrier : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Destroy(collision.gameObject);  // Détruire la balle
            GameManager.instance.RespawnBall();  // Faire respawn une nouvelle balle
        }
    }
}
