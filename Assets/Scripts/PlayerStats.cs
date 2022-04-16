using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats playerStats;
    [HideInInspector]
    public PlayerController playerController;
    public int healthPoints;
    public int shock;

    private void Awake()
    {
        playerStats = this;
    }
}
