using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityRoyale
{
    [RequireComponent(typeof(UIDocument))]
    public class GameScreen : UIController<GameScreenVM>
    {
        [SerializeField] PlayingAreaUI _playingAreaUI;

        VisualElement _cardPanel;
        DeckPileElement _deckPile;
        PlayingAreaElement _playingArea;

        public VisualElement CardPanel => _cardPanel;
        public DeckPileElement DeckPile => _deckPile;
        public PlayingAreaElement PlayingArea => _playingArea;

        public PlayingAreaUI PlayingAreaUI => _playingAreaUI;

        void OnEnable()
        {
            var root = view.rootVisualElement;
            _cardPanel = root.Q("cardpanel");
            _deckPile = root.Q<DeckPileElement>();
            _playingArea = root.Q<PlayingAreaElement>();

            _playingAreaUI.Initialize(_playingArea);

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
