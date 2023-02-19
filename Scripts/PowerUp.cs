using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Component Reference")]
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").gameObject.GetComponent<GameManager>();
    }

    public void UpAutomaticPoints()
    {
        _gameManager.SetUpAutomaticPoints();
    }

    public void UpPointsForClick()
    {
        _gameManager.SetPointsToUp();
    }

    public void DownResistance()
    {
        _gameManager.SetResistance();
    }

    public void DrugsAutomatic()
    {
        _gameManager.SetAutomaticDrug();
    }
}
