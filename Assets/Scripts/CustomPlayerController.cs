using UnityEngine;

public class CustomPlayerController : MonoBehaviour
{
    public float speed = 5f;  // Vitesse de déplacement
    public KeyCode moveLeftKey = KeyCode.A;  // Touche pour aller à gauche
    public KeyCode moveRightKey = KeyCode.D;  // Touche pour aller à droite

    public Transform leftBarrier;  // Référence à la barrière gauche
    public Transform rightBarrier;  // Référence à la barrière droite

    public float margin = 0.1f;  // Marge pour s'arrêter avant les barrières

    private float objectWidth;

    void Start()
    {
        // Calculer la moitié de la largeur de l'objet à partir de son Collider
        objectWidth = GetComponent<Collider>().bounds.extents.x;
    }

    void Update()
    {
        float horizontalInput = 0;

        // Obtenir l'entrée de l'utilisateur basée sur les touches configurées
        if (Input.GetKey(moveLeftKey))
        {
            horizontalInput = -1;
        }
        if (Input.GetKey(moveRightKey))
        {
            horizontalInput = 1;
        }

        // Calculer le déplacement
        Vector3 movement = new Vector3(horizontalInput, 0, 0) * speed * Time.deltaTime;

        // Calculer la nouvelle position potentielle
        Vector3 newPosition = transform.position + movement;

        // Limiter la position pour éviter de traverser les barrières
        newPosition.x = Mathf.Clamp(newPosition.x, leftBarrier.position.x + objectWidth + margin, rightBarrier.position.x - objectWidth - margin);

        // Appliquer le déplacement à l'objet
        transform.position = newPosition;
    }
}
