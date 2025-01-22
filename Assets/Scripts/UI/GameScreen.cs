using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityRoyale
{
    [RequireComponent(typeof(UIDocument))]
    public class GameScreen : UIController<GameScreenVM>
    {
        VisualElement _cardPanel, _backupPanel, _activePanel;
        DeckPileElement _deckPile;

        public VisualElement CardPanel => _cardPanel;
        public DeckPileElement DeckPile => _deckPile;
        public VisualElement ActivePanel => _activePanel;

        void OnEnable()
        {
            var root = view.rootVisualElement;
            _cardPanel = root.Q("cardpanel");
            //_cardPanel.style.display = DisplayStyle.None;
            //_backupPanel = root.Q("backup");
            _deckPile = root.Q<DeckPileElement>();
            _activePanel = root.Q("active");

            viewModel.IsVisible.Value = false;
        }

        public VisualElement GetCardPanelRoot()
        {
            // Enable the screen with the first access to the panel, as it means we want to show cards.
            ShowGameScreen();

            return _cardPanel;
        }

        public void HideGameScreen()
        {
            //_cardPanel.style.display = DisplayStyle.None;
            viewModel.IsVisible.Value = false;
        }

        public void ShowGameScreen()
        {
            //_cardPanel.style.display = DisplayStyle.Flex;
            viewModel.IsVisible.Value = true;
        }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        public static void RegisterConverters()
        {
            var group = new ConverterGroup("Bool to DisplayStyle");
            group.AddConverter((ref bool b) => b ? new StyleEnum<DisplayStyle>(DisplayStyle.Flex) 
                                                 : new StyleEnum<DisplayStyle>(DisplayStyle.None));
            ConverterGroups.RegisterConverterGroup(group);
        }
    }
}
