using UnityEngine;
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
            _healthUI.AddHealthBar(p);
        }

        public void DisableHealthUI()
        {
            _healthUI.ResetUI();
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