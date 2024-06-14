using UnityEngine;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    public static string playerName;
    public TMP_InputField inputField; // Assurez-vous que cela est assigné dans l'inspecteur

    void Start()
    {
        // Vérifiez que le champ inputField est bien assigné
        if (inputField == null)
        {
            inputField = GetComponent<TMP_InputField>();
            if (inputField == null)
            {
                Debug.LogError("TMP_InputField component not found.");
                return;
            }
        }

        inputField.onEndEdit.AddListener(SetPlayerName);
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public static void ClearPlayerName()
    {
        playerName = string.Empty;
    }

    public static void ShowInputField()
    {
        GameObject inputFieldObj = GameObject.Find("PlayerNameInputField");
        if (inputFieldObj != null)
        {
            inputFieldObj.SetActive(true);
        }
        else
        {
            Debug.LogError("PlayerNameInputField GameObject not found.");
        }
    }

    public static void HideInputField()
    {
        GameObject inputFieldObj = GameObject.Find("PlayerNameInputField");
        if (inputFieldObj != null)
        {
            inputFieldObj.SetActive(false);
        }
        else
        {
            Debug.LogError("PlayerNameInputField GameObject not found.");
        }
    }
}
