using UnityEngine;

public class Brick : MonoBehaviour
{
    public int durability;
    public Color[] durabilityColors; // Tableau de couleurs en fonction de la durabilité

    private Renderer brickRenderer;

    void Start()
    {
        brickRenderer = GetComponent<Renderer>();
        UpdateColor();
    }

    // Méthode pour réduire la durabilité et détruire la brique si nécessaire
    public void Hit()
    {
        durability--;
        if (durability <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.BrickDestroyed(); // Notifier le GameManager
        }
        else
        {
            UpdateColor();
        }
    }

    // Méthode pour mettre à jour la couleur en fonction de la durabilité
    void UpdateColor()
    {
        if (durability - 1 < durabilityColors.Length)
        {
            brickRenderer.material.color = durabilityColors[durability - 1];
        }
    }
}
