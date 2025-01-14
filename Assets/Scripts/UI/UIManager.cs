﻿using UnityEngine;
using UnityEngine.UIElements;

namespace UnityRoyale
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameScreen gameScreen = default;
        [SerializeField] private EndScreen endScreenPrefab = default;
        [SerializeField] HealthUIManager _healthUI;

        public void AddHealthUI(ThinkingPlaceable p)
        {
            //var healthBar = Instantiate(healthBarPrefab, healthBarContainer);
            //p.healthBar = healthBar;
            //healthBar.Initialize(p);

            var healthBar = _healthUI.AddHealthBar(p);
            p.healthBar = healthBar;
        }

        public void RemoveHealthUI(ThinkingPlaceable p)
        {
            if (p.healthBar)
            {
                Destroy(p.healthBar.gameObject);
            }
        }

        public VisualElement GetCardPanelRoot()
        {
            return gameScreen.GetCardPanelRoot();
        }

        public void ShowGameOverUI(string winnerTeamName)
        {
            gameScreen.HideGameScreen();
            EndScreen.Show(endScreenPrefab, transform, winnerTeamName);
        }
    }
}