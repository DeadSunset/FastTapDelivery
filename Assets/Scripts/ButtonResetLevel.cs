using System.Collections.Generic;
using UnityEngine;

public class ButtonResetLevel : MonoBehaviour
{
    public List<GameObject> buttonsToReset = new List<GameObject>();
    public void Reset()
    {
        GameEvents.events.MapRestart();
        if (!GameManager.game.onMenuOpened)
        {
            foreach (var item in MapGenerator.map.fieldList)
            {
                Destroy(item);
            }
            MapGenerator.map.fieldList.Clear();

            foreach (var item in MapGenerator.map.roadPathList)
            {
                Destroy(item);
            }
            MapGenerator.map.roadPathList.Clear();

            foreach (var item in EnemyManager.enemyManager.allActiveEnemies)
            {
                Destroy(item.gameObject);
            }
            EnemyManager.enemyManager.allActiveEnemies.Clear();

            if (PlayerStats.playerStats.playerController)
            {
                Destroy(PlayerStats.playerStats.playerController.gameObject);
                PlayerStats.playerStats.playerController = null;
            }

            MapGenerator.map.GenerateMap();

            foreach (var item in buttonsToReset)
            {
                item.SetActive(false);
                item.SetActive(true);
            }

            MapGenerator.map.isRoadEnded = false;
            GameManager.game.currentTool = "empty";

            CameraManager.cameraManager.EnableMainCamera(true);
        }
    }
}
