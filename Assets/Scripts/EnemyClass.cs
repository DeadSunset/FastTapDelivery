using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyClass : MonoBehaviour
{
    public int ID;
    public string enemyName;
    public float damage;
    public float health;
    public float secondsToAttack;

    public bool isDead;

    private GameObject _coolDownCircle;

    private GameObject _toDestroy;

    private void Start()
    {
        print("spawned");
        MoveForFight();
        _coolDownCircle = EnemyCoolDownCircle.circle;
        _toDestroy = gameObject;
        GameEvents.events.DealDamageToEnemy();
        var roadTrans = MapGenerator.map.roadPathList[ID].transform.rotation;
        gameObject.transform.rotation = new Quaternion(gameObject.transform.rotation.x, roadTrans.y, gameObject.transform.rotation.z, 0);
    }

    private void FixedUpdate()
    {
        if (PlayerStats.playerStats.playerController)
        {
            if (Vector3.Distance(PlayerStats.playerStats.playerController.transform.position, transform.position) > 0.7f)
                transform.LookAt(PlayerStats.playerStats.playerController.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats.playerStats.playerController.currentEnemies.Add(this);
            PlayerStats.playerStats.StartFight(this);
            StartCoroutine(MoveToSide());
            InvokeRepeating("Attack", 0f, secondsToAttack);
            GameEvents.events.DisableAllEnemiesExcept(this);
        }
    }

    private void Attack()
    {
        _coolDownCircle.GetComponent<Image>().fillAmount = 0;
        PlayerStats.playerStats.HealthDecreased(damage);
        _coolDownCircle.GetComponent<Image>().DOFillAmount(3, secondsToAttack);
    }

    public bool HealthDecreased(float fullfilment)
    {
        health -= fullfilment;
        GameEvents.events.DealDamageToEnemy();
        if (health <= 0)
        {
            isDead = true;
            return true;
        }
        else return false;

    }

    public void StopAttack()
    {
        StartCoroutine(AttackStoped());
    }

    private IEnumerator AttackStoped()
    {
        CancelInvoke("Attack");
        _coolDownCircle.GetComponent<Image>().DOKill();
        _coolDownCircle.GetComponent<Image>().fillAmount = 0;

        yield return new WaitForSeconds(1f);
        InvokeRepeating("Attack", 0f, secondsToAttack);
    }

    private IEnumerator DestroyDelay()
    {
        CancelInvoke("Attack");
        yield return new WaitForSeconds(1f);
        InvokeRepeating("Attack", 0f, secondsToAttack);
    }

    private void MoveForFight()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.12f, gameObject.transform.position.z);
    }

    private IEnumerator MoveToSide()
    {
        yield return new WaitForSeconds(0.5f);
        var player = PlayerStats.playerStats.playerController;
        var enemy = PlayerStats.playerStats.playerController.currentEnemies[0];
        if (player.transform.rotation.eulerAngles.y == 90 || player.transform.rotation.eulerAngles.y == -270)
        {
            enemy.transform.DOMove(new Vector3(gameObject.transform.position.x + 1f, gameObject.transform.position.y, gameObject.transform.position.z), 0.3f);
        }
        else if (player.transform.rotation.eulerAngles.y == -180 || player.transform.rotation.eulerAngles.y == 180)
        {
            enemy.transform.DOMove(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 1f), 0.3f);
        }
        else if (player.transform.rotation.eulerAngles.y == 0)
        {
            enemy.transform.DOMove(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 1f), 0.3f);
        }
        else if (player.transform.rotation.eulerAngles.y == -90 || player.transform.rotation.eulerAngles.y == 270)
        {
            enemy.transform.DOMove(new Vector3(gameObject.transform.position.x - 1f, gameObject.transform.position.y, gameObject.transform.position.z), 0.3f);
        }
    }
}
