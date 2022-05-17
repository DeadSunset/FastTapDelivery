using UnityEngine;
using UnityEngine.UI;

public class DishClass : MonoBehaviour
{
    public IngredientCardClass[] ingredientList;
    public float dishFullfilment;

    [SerializeField]
    private Text _dishFullfilmentText;
    [SerializeField]
    private GameObject _rndFood;
    [SerializeField]
    private AudioClip _feedSound;
    [SerializeField]
    private AudioClip _rejectSound;

    private int _activeIngredients;
    private AudioSource _audio;
    private void Start()
    {
        _audio = gameObject.AddComponent<AudioSource>();
        ingredientList = gameObject.GetComponentsInChildren<IngredientCardClass>();
        GameEvents.events.OnIngredientTake += OnValueChanged;
        GameEvents.events.OnFightStart += RandomizeCurrentIngredients;
    }

    private void RandomizeCurrentIngredients()
    {
        foreach (var ingredient in ingredientList)
        {
            ingredient.gameObject.SetActive(true);
        }
        for (int i = 0; i < 3; i++)
        {
            if (Random.Range(1, 3) % 2 == 0)
            {
                ingredientList[Random.Range(0, ingredientList.Length)].gameObject.SetActive(false);
            }
        }
    }
    public void OnValueChanged()
    {
        _activeIngredients = 0;
        dishFullfilment = 0;
        foreach (var ingredient in ingredientList)
        {
            if (ingredient.isGoingToDish)
            {
                dishFullfilment += ingredient.hungerFullfilment;
                _activeIngredients++;
            }
        }
        _dishFullfilmentText.text = dishFullfilment.ToString();
        if (_activeIngredients > 2)
        {
            _rndFood.GetComponent<ShowRandomFood>().TakeRandom();
        }
        else
        {
            _rndFood.GetComponent<ShowRandomFood>().TurnOff();
        }
    }

    public void OnClick()
    {
        if (_activeIngredients > 2)
        {
            _audio.clip = _feedSound;
            foreach (var ingredient in ingredientList)
            {
                ingredient.DisableOutlines();
                ingredient.isGoingToDish = false;
            }
            foreach (var enemy in PlayerStats.playerStats.playerController.currentEnemies)
            {
                if (enemy.HealthDecreased(dishFullfilment))
                {
                    GameEvents.events.DealDamageToEnemy();
                    if (enemy.isDead)
                    {
                        EnemyManager.enemyManager.EnemyDeadFreeRoad(enemy);
                        EnemyManager.enemyManager.allActiveEnemies.Remove(enemy);
                    }
                }
                enemy.StopAttack();
            }
            var enems = PlayerStats.playerStats.playerController.currentEnemies.FindAll(item => item.isDead == true);
            foreach (var e in enems)
            {
                PlayerStats.playerStats.playerController.currentEnemies.Remove(e);
                Destroy(e.gameObject);
            }
            if (PlayerStats.playerStats.playerController.currentEnemies.Count == 0)
            {
                GameEvents.events.FightEnd(true);
            }
            OnValueChanged();
        }
        else
        {
            _audio.clip = _rejectSound;
        }
        _audio.Play();
    }


}
