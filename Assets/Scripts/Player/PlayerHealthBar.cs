﻿using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject _healthUnit;
    [SerializeField] private float _distanceBetweenUnits;

    private Player.Player _player;
    public static bool ShouldUpdateHealth;


    private void OnEnable()
    {
        Player.Player.OnDamage += DrawHealthUnits;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player.Player>();
        DrawHealthUnits();
    }

    private void OnDisable()
    {
        Player.Player.OnDamage -= DrawHealthUnits;
    }

    private void DrawHealthUnits()
    {
        GameObject[] healthUnits = GameObject.FindGameObjectsWithTag("HealthUnit");
        for (int i = 0; i < healthUnits.Length; i++)
        {
            Destroy(healthUnits[i].gameObject);
        }

        float initialXPosition = 0;

        for (int i = 0; i < _player.Health; i++)
        {
            GameObject healthUnit = Instantiate(_healthUnit, transform, false);
            healthUnit.transform.localPosition = new Vector2(initialXPosition, 0);
            initialXPosition += _distanceBetweenUnits;
        }
    }
}
