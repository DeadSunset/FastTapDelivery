using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats playerStats;
    [HideInInspector]
    public PlayerController playerController;
    public int maxHealthPoints;
    public int maxShock;

    public int currentHealthPoints;
    public int currentShock;

    [SerializeField]
    private float _secondsToDestroy;
    [SerializeField]
    private Slider _shockSlider;

    private HeartSetImages _heartSet;
    private PlayerEffects _playerEffects;

    [SerializeField]
    private AudioClip _joyFed;
    [SerializeField]
    private AudioClip _takeDamage;
    [SerializeField]
    private AudioClip _heartDroped;

    private AudioSource _audio;


    private void Awake()
    {
        _shockSlider.maxValue = maxShock;
        _shockSlider.value = maxShock;
        currentHealthPoints = maxHealthPoints;
        _playerEffects = gameObject.GetComponent<PlayerEffects>();
        playerStats = this;
        _heartSet = gameObject.GetComponent<HeartSetImages>();
    }

    private void Start()
    {

        GameEvents.events.OnFightEnd += FightEnd;

        _audio = gameObject.AddComponent<AudioSource>();
        GameEvents.events.OnFightEnd += EnemyFed;

        GameEvents.events.OnFightEnd += Heal;
    }

    public void StartFight(EnemyClass enemy)
    {
        _playerEffects = playerController.GetComponent<PlayerEffects>();
        GameEvents.events.FightStart();
        playerController.StopAllCoroutines();
        playerController.gameObject.GetComponent<Animator>().Play("Idle");
        if (playerController != null)
        {
            CameraManager.cameraManager.ChangeCameraViewToNewCamera(playerController.fightCamera);
            CameraManager.cameraManager.canvasFight.enabled = true;
            CameraManager.cameraManager.canvasMain.enabled = false;
        }
    }

    public void HealthDecreased(float getDamage)
    {
        currentShock -= System.Convert.ToInt32(getDamage);
        UpdateSlider();
        _audio.clip = _takeDamage;
        if (currentShock <= 0)
        {
            _audio.clip = _heartDroped;
            if (_heartSet.heartsList.Count > 1)
            {
                currentShock = maxShock;
                DestroyHeart();
                _playerEffects.TakeDamageEffect();
            }
            else
            {
                GameEvents.events.GameEnd(false);
                _playerEffects.DeathEffect();
                playerController.gameObject.transform.DOScale(Vector3.zero, _secondsToDestroy);
                StartCoroutine(DestroyDelayed());
            }

        }
        _audio.Play();
    }

    public void FightEnd(bool win)
    {
        if (win)
        {
            CameraManager.cameraManager.canvasMain.enabled = true;
            CameraManager.cameraManager.canvasFight.enabled = false;
            StartCoroutine(BackToRun());
        }
    }

    private IEnumerator DestroyDelayed()
    {
        CameraManager.cameraManager.EnableMainCamera(false);
        CameraManager.cameraManager.canvasFight.enabled = false;
        CameraManager.cameraManager.canvasStats.enabled = false;
        yield return new WaitForSeconds(_secondsToDestroy);
        playerController.StopAllCoroutines();
        Destroy(playerController.gameObject);

    }

    private void EnemyFed(bool win)
    {
        if (win)
        {
            _audio.clip = _joyFed;
            _audio.Play();
        }
    }

    private void DestroyHeart()
    {
        var index = _heartSet.heartsList.Count - 1;
        Destroy(_heartSet.heartsList[index]);
        _heartSet.heartsList.RemoveAt(index);
    }

    private IEnumerator BackToRun()
    {
        yield return new WaitForSeconds(1f);
        if (playerController != null)
        {
            playerController.StartMoving();
            playerController.gameObject.GetComponent<Animator>().Play("Run");
        }
    }

    private void UpdateSlider()
    {
        _shockSlider.maxValue = maxShock;
        _shockSlider.value = currentShock;
    }

    private void Heal(bool win)
    {
        _shockSlider.value = _shockSlider.maxValue;
    }

}
