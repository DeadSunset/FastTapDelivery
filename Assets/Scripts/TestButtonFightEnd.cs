using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtonFightEnd : MonoBehaviour
{
    public void OnClick()
    {
        GameEvents.events.FightEnd(true);
    }
}
