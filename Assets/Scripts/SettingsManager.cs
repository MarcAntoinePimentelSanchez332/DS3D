using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public CustomPlayerController playerController;
    public InputField moveLeftInputField;
    public InputField moveRightInputField;
    public InputField speedInputField;

    void Start()
    {
        // Initialiser les champs de texte avec les valeurs actuelles
        moveLeftInputField.text = playerController.moveLeftKey.ToString();
        moveRightInputField.text = playerController.moveRightKey.ToString();
        speedInputField.text = playerController.speed.ToString();
    }

    public void ApplySettings()
    {
        // Appliquer les touches personnalisées
        if (moveLeftInputField.text.Length == 1)
        {
            playerController.moveLeftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), moveLeftInputField.text.ToUpper());
        }
        if (moveRightInputField.text.Length == 1)
        {
            playerController.moveRightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), moveRightInputField.text.ToUpper());
        }

        // Appliquer la vitesse personnalisée
        float speed;
        if (float.TryParse(speedInputField.text, out speed))
        {
            playerController.speed = speed;
        }
    }
}
