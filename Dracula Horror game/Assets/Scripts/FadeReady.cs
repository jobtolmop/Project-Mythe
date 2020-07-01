using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeReady : MonoBehaviour
{
    public void LoadReady()
    {
        GameManager.instance.ReadyLoading = true;
    }
}
