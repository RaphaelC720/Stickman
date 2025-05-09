using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillbar;
    public PlayerScript Player;

    void Update()
    {
        float healthPercent = Mathf.Clamp01(Player.Health / Player.Maxhealth);
        fillbar.fillAmount = healthPercent;
    }
}
