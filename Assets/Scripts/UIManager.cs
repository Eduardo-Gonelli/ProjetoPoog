using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI playerHealthText;
    public PlayerData playerData;
    // Start is called before the first frame update
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHealthText()
    {
        playerHealthText.text = "Health: " + playerData.health;
    }
}
