using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCoolDownCircle : MonoBehaviour
{
    public static GameObject circle;

    private void Awake()
    {
        circle = gameObject;
    }
}
