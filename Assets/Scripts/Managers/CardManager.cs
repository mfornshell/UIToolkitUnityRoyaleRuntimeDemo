using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UIElements;
using System;

namespace UnityRoyale
{
    public class CardManager : MonoBehaviour
    {
        GameScreen _gameScreen;

        // Prefab properties
        [SerializeField] private Camera mainCamera = default;
        [SerializeField] private LayerMask playingFieldMask = default;
        //[SerializeField] private VisualTreeAsset visualTreeCard = default;
        [SerializeField] private DeckData playersDeck = default;
        [SerializeField] CardUI _cardPrefab;
        [SerializeField] private MeshRenderer forbiddenAreaRenderer = default;
		
        public UnityAction<CardData, Vector3, Placeable.Faction> OnCardUsed;
        
        private CardElement[] cards;
        private int cardCount = 3;
        private bool cardIsActive = false; //when true, a card is being dragged over the play field
        private GameObject previewHolder;
        private readonly Vector3 InputCreationOffset = new Vector3(0f, 0f, 1f); //offsets the creation of units so that they are not under the player's finger

        int _playAreaCount;
        bool _deckReady;

        private void Awake()
        {
            previewHolder = new GameObject("PreviewHolder");
            cards = new CardElement[cardCount]; //3 is the length of the dashboard
        }

        private void Start()
        {
            _gameScreen = UIManager.Instance.GameScreen;
        }

        public void LoadDeck()
        {
            DeckLoader newDeckLoaderComp = gameObject.AddComponent<DeckLoader>();
            newDeckLoaderComp.OnDeckLoaded += DeckLoaded;
            newDeckLoaderComp.LoadDeck(playersDeck);
        }


        private void DeckLoaded()
        {
            Debug.Log("Player's deck loaded");

            _gameScreen.ShowGameScreen();

            StartCoroutine(FillPlayArea());
        }

        IEnumerator FillPlayArea()
        {
            while (true)
            {
                if (!_deckReady)
                {
                    yield return AddCardToDeck(.4f);
                }
                else if (CanFill(out int index))
                {
                    yield return PromoteCardFromDeck(index, .8f);
                }
                else yield return null;
            }
            
            bool CanFill(out int index)
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    if (cards[i] == null)
                    {
                        index = i;
                        return true;
                    }
                }
                index = -1;
                return false;
            }
        }

        private VisualElement GetActiveContainer()
        {
            return _gameScreen.ActivePanel;
        }

        IEnumerator MoveToPlayArea(int index, float delay = .4f)
        {
            var card = _gameScreen.DeckPile.Card;
            card.Index = index;
            cards[index] = card;

            yield return null;
        }

        void RegisterCardCallbacks(CardElement card)
        {
            card.RegisterCallback<MouseDownEvent>(evt => CardTapped(evt, card.Index));
            card.RegisterCallback<MouseUpEvent>(evt => CardReleased(evt, card.Index));
            card.RegisterCallback<MouseMoveEvent>(evt => CardDragged(evt, card.Index));
        }

        //moves the preview card from the deck to the active card dashboard
        private IEnumerator PromoteCardFromDeck(int cardId, float delay = 0f)
        {
            yield return new WaitForSecondsRealtime(delay);

            var deckPile = _gameScreen.DeckPile;
            //var backupCard = backupContainer.Q<CardElement>();
            var card = deckPile.Card;

            card.Index = cardId;

            
            Vector2 screenPosition = card.LocalToWorld(card.transform.position);

            var activePanel = GetActiveContainer();
            activePanel.Add(card);

            card.MoveTo(card.WorldToLocal(screenPosition));
            card.MoveAndScaleIntoPosition(cardId, ComputeActiveCardPosition(cardId));

            // TODO need to wait for MoveAndScale, use an event callback to set MouseEvents 
            //yield return new WaitForSeconds(.4f);

            card.RegisterCallback<MouseDownEvent>(evt => CardTapped(evt, cardId));
            card.RegisterCallback<MouseUpEvent>(evt => CardReleased(evt, cardId));
            card.RegisterCallback<MouseMoveEvent>(evt => CardDragged(evt, cardId));

            //store a reference to the Card component in the array
            cards[cardId] =  card;
            _deckReady = false;
        }

        //adds a new card to the deck on the left, ready to be used
        private IEnumerator AddCardToDeck(float delay = 0f) //TODO: pass in the CardData dynamically
        {
            yield return new WaitForSecondsRealtime(delay);

            var card = Instantiate(_cardPrefab, _gameScreen.transform);
            card.Initialize(playersDeck.GetNextCardFromDeck());

            _gameScreen.DeckPile.AddCard(card.CardElement);

            _deckReady = true;
        }

        private int draggedCardId = -1;
        private Vector2 mouseDownPosition;
        private void CardTapped(MouseDownEvent clickEvent, int cardId)
        {
            cards[cardId].SetAsLastSibling();
			forbiddenAreaRenderer.enabled = true;

            draggedCardId = cardId;
            mouseDownPosition = clickEvent.mousePosition;
        }

        private void CardDragged(IMouseEvent dragEvent, int cardId)
        {
            if (cardId != draggedCardId)
                return;

            var dragAmount = dragEvent.mousePosition - mouseDownPosition;
            cards[cardId].Translate(dragAmount);
            mouseDownPosition = dragEvent.mousePosition;

            //raycasting to check if the card is on the play field
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            bool planeHit = Physics.Raycast(ray, out hit, Mathf.Infinity, playingFieldMask);

            if (planeHit)
            {
                if (!cardIsActive)
                {
                    cardIsActive = true;
                    previewHolder.transform.position = hit.point;
                    cards[cardId].ChangeActiveState(true); //hide card

                    //retrieve arrays from the CardData
                    PlaceableData[] dataToSpawn = cards[cardId].cardData.placeablesData;
                    Vector3[] offsets = cards[cardId].cardData.relativeOffsets;

                    //spawn all the preview Placeables and parent them to the cardPreview
                    for (int i = 0; i < dataToSpawn.Length; i++)
                    {
                        Instantiate(dataToSpawn[i].associatedPrefab, hit.point + offsets[i] + InputCreationOffset,
                            Quaternion.identity, previewHolder.transform);
                    }
                }
                else
                {
                    //temporary copy has been created, we move it along with the cursor
                    previewHolder.transform.position = hit.point;
                }
            }
            else
            {
                if (cardIsActive)
                {
                    cardIsActive = false;
                    cards[cardId].ChangeActiveState(false); //show card

                    ClearPreviewObjects();
                }
            }
        }

        private void CardReleased(MouseUpEvent mouseUpEvent, int cardId)
        {
            //raycasting to check if the card is on the play field
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, playingFieldMask))
            {
                //GameManager registers to OnCardUsed to spawn the actual Placeable
                OnCardUsed?.Invoke(cards[cardId].cardData, hit.point + InputCreationOffset,
                    Placeable.Faction.Player);

                ClearPreviewObjects();
                cards[cardId].Delete(); //remove the card itself
                cards[cardId] = null;
                
                //StartCoroutine(PromoteCardFromDeck(cardId, .2f));
                //StartCoroutine(AddCardToDeck(.6f));
            }
            else
            {
                cards[cardId].AnimatedMoveTo(ComputeActiveCardPosition(cardId),.2f);
            }

            forbiddenAreaRenderer.enabled = false;
            draggedCardId = -1;
        }

        private Vector2 ComputeActiveCardPosition(int cardId)
        {
            var activePanel = GetActiveContainer();
            const float margin = 10f;
            float offset = margin + cardId * (activePanel.layout.width - 2*margin) / cardCount;
            return new Vector2(offset, 0);
        }
        
        //happens when the card is put down on the playing field, and while dragging (when moving out of the play field)
        private void ClearPreviewObjects()
        {
            //destroy all the preview Placeables
            for (int i = 0; i < previewHolder.transform.childCount; i++)
            {
                Destroy(previewHolder.transform.GetChild(i).gameObject);
            }
        }
    }

}
