using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{

    public static GameEvents events;

    public event Action OnDealDamageToEnemy;

    public event Action OnIngredientTake;

    public event Action<int> OnGetSquareId;
    public event Action OnToolIconResize;
    public event Action OnRoadEnded;
    public event Action OnRoadTileSet;
    public event Action OnPlayerMoved;

    public event Action OnPointIsReady;

    public event Action OnMapRestart;


    public event Action OnFightStart;
    public event Action<bool> OnFightEnd;

    public event Action<bool> OnGameEnd;

    public event Action<EnemyClass> OnDisableAllEnemiesExcept;

    public event Action OnMenuOpened;
    public event Action OnMenuClosed;

    public event Action<string> OnSetCurrentTool;
    public event Action<Vector3> OnPassCoordinates;
    public void GetSquareId(int id) => OnGetSquareId?.Invoke(id);
    public void ToolIconResize() => OnToolIconResize?.Invoke();
    public void IngredientTake() => OnIngredientTake?.Invoke();
    public void RoadEnded() => OnRoadEnded?.Invoke();

    public void MapRestart() => OnMapRestart?.Invoke();

    public void DealDamageToEnemy() => OnDealDamageToEnemy?.Invoke();

    public void MenuOpened() => OnMenuOpened?.Invoke();
    public void MenuClosed() => OnMenuClosed?.Invoke();

    public void PointIsReady() => OnPointIsReady?.Invoke();

    public void FightStart() => OnFightStart?.Invoke();
    public void FightEnd(bool win) => OnFightEnd?.Invoke(win);

    public void GameEnd(bool win) => OnGameEnd?.Invoke(win);

    public void DisableAllEnemiesExcept(EnemyClass enemy) => OnDisableAllEnemiesExcept?.Invoke(enemy);

    public void RoadTileSet() => OnRoadTileSet?.Invoke();
    public void PlayerMoved() => OnPlayerMoved?.Invoke();
    public void SetCurrentTool(string tool) => OnSetCurrentTool?.Invoke(tool);
    public void PassCoordinates(Vector3 pos) => OnPassCoordinates?.Invoke(pos);

    private void Awake()
    {
        events = this;
    }
}
