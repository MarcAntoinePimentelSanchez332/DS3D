using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickGenerator : MonoBehaviour
{
    public GameObject brickPrefab;
    public GameObject platform; // Référence à l'objet plateau
    public Transform bricksContainer; // Référence au conteneur des briques
    public int rows = 6; // Nombre de rangées sur l'axe Z
    public int columns = 10; // Nombre de colonnes sur l'axe X
    public float fillProbability = 0.7f; // Probabilité de générer une brique à une position donnée
    public int minDurability = 1;
    public int maxDurability = 5;
    public Color[] durabilityColors; // Tableau de couleurs en fonction de la durabilité

    void Start()
    {
        GenerateBricks();
    }

    public void GenerateBricks()
    {
        if (platform == null || brickPrefab == null || bricksContainer == null)
        {
            Debug.LogError("Platform, BrickPrefab, or BricksContainer is not assigned.");
            return;
        }

        // Supprimer les anciennes briques
        foreach (Transform child in bricksContainer)
        {
            Destroy(child.gameObject);
        }

        // Obtenir les dimensions du plateau
        Vector3 platformSize = platform.GetComponent<Collider>().bounds.size;
        Debug.Log("Platform size: " + platformSize);

        // Calculer la largeur et la profondeur des briques
        float brickWidth = brickPrefab.GetComponent<Renderer>().bounds.size.x;
        float brickDepth = brickPrefab.GetComponent<Renderer>().bounds.size.z;
        Debug.Log("Brick size: " + brickWidth + ", " + brickDepth);

        // Calculer le nombre de colonnes en fonction de la largeur du plateau et de la largeur des briques
        int columns = Mathf.FloorToInt(platformSize.x / brickWidth);
        Debug.Log("Number of columns: " + columns);

        // Générer les briques de manière aléatoire
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                // Décider aléatoirement s'il faut générer une brique à cette position
                if (Random.value < fillProbability)
                {
                    // Calculer la position de chaque brique
                    Vector3 position = new Vector3(
                        platform.transform.position.x - platformSize.x / 2 + brickWidth / 2 + column * brickWidth,
                        platform.transform.position.y + platformSize.y / 2 + brickPrefab.GetComponent<Renderer>().bounds.size.y / 2,
                        platform.transform.position.z - platformSize.z / 2 + brickDepth / 2 + row * brickDepth
                    );

                    Debug.Log("Creating brick at position: " + position);

                    GameObject brick = Instantiate(brickPrefab, position, Quaternion.identity);
                    brick.transform.parent = bricksContainer; // Assigner le conteneur comme parent
                    Brick brickScript = brick.GetComponent<Brick>();
                    brickScript.durability = Random.Range(minDurability, maxDurability + 1);
                    brickScript.durabilityColors = durabilityColors; // Assigner les couleurs de durabilité
                }
            }
        }
    }
}
