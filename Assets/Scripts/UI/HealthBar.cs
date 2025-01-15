using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityRoyale
{
    [RequireComponent(typeof(UIDocument))]
    public class HealthBar : UIController<HealthBarVM>
    {
        private const string HiddenHealthBarStyleClass = "health-bar--hidden";

        ThinkingPlaceable _healthModelPlaceable; // TODO : replace this with a proper healthModel

        // Prefab properties
        //[SerializeField] private Vector3 unitAnchorPosition = new Vector3(0f, 0f, 0f);
        //[SerializeField] private Vector3 nonUnitAnchorPosition = new Vector3(0f, 0f, 0f);
        [SerializeField] private Vector2 worldSize = new Vector2(1f, 1f);
        //[SerializeField] private Color red = new Color32(252, 35, 13, 255);
        //[SerializeField] private Color blue = new Color32(31, 132, 255, 255);

        //[SerializeField] private bool isHidden = true;

        //[HideInInspector] public float originalHealth;
        //[HideInInspector] public float currentHealth;
        [HideInInspector] public Vector3 anchorPosition;
        //[HideInInspector] public Color barColor;
        [HideInInspector] public Transform transformToFollow;

        [SerializeField] GameObject _followingObject;
        [SerializeField] string _followingObjectName;
        [SerializeField] Vector3 _currentPos;

        private VisualElement _healthBarElement;
        
        /*
        public void Initialize(ThinkingPlaceable p)
        {            
            currentHealth = originalHealth = p.hitPoints;
            anchorPosition = p.pType == Placeable.PlaceableType.Unit ? unitAnchorPosition : nonUnitAnchorPosition;
            barColor = p.faction == Placeable.Faction.Player ? red : blue;
            transformToFollow = p.transform;
            _followingObject = p.gameObject;
            _followingObjectName = p.gameObject.name;
        }
        */
        public void Initialize(ThinkingPlaceable p, Vector3 anchor, float hitPoints, Color color)
        {
            viewModel.Initialize(hitPoints, color);
            _healthModelPlaceable = p;
            _healthModelPlaceable.HealthChanged += viewModel.CurrentHealth;
            _healthModelPlaceable.OnDie += Remove;
            viewModel.CurrentHealth.OnValueChanged += OnHealthChanged;
            

            //currentHealth = originalHealth = hitPoints;
            anchorPosition = anchor;
            //barColor = color;
            transformToFollow = p.transform;
            _followingObject = p.gameObject;
            _followingObjectName = p.gameObject.name;
        }

        void OnEnable()
        {
            _healthBarElement = view.rootVisualElement.Q("HealthBar");
            viewModel = ScriptableObject.CreateInstance<HealthBarVM>();
            _healthBarElement.dataSource = viewModel;
        }

        private void Start()
        {
            //viewModel = ScriptableObject.CreateInstance<HealthBarVM>();
            //_healthBarElement.dataSource = viewModel;

            //bar.style.unityBackgroundImageTintColor = barColor;
            //SetHealth(currentHealth);
        }

        void OnHealthChanged(float newHealth)
        {
            //currentHealth = newHealth;
            //viewModel.CurrentHealth.Value = newHealth;
            //isHidden = newHealth >= originalHealth;
            viewModel.IsVisible.Value = newHealth < viewModel.OriginalHealth;

            //float ratio = newHealth > 0f ? newHealth / originalHealth : 1e-5f;
            //bar.transform.scale = new Vector3(ratio, 1, 1);
            
            // Hide the health bar after the position is set, otherwise it won't hide.
            //_healthBarElement.EnableInClassList(HiddenHealthBarStyleClass, isHidden);
        }

        private void Update()
        {
            //wholeWidget.EnableInClassList(HiddenHealthBarStyleClass, isHidden);
        }

        // Wait for LateUpdate 1) to allow tracked object to move and
        //                     2) to leave time for wholeWidget.layout to be refreshed
        private void LateUpdate()
        {
            if (viewModel.IsVisible && transformToFollow != null)
            {
                MoveAndScaleToWorldPosition(_healthBarElement, transformToFollow.position + anchorPosition, worldSize);
                //MoveTo(wholeWidget, transformToFollow.position + anchorPosition);
                _currentPos = _healthBarElement.transform.position;
            }
        }

        void MoveTo(VisualElement widget, Vector3 worldPos)
        {
            var pos = RuntimePanelUtils.CameraTransformWorldToPanel(widget.panel, worldPos, Camera.main);

            var layoutSize = widget.layout.size;

            var scale = pos / layoutSize;

            widget.transform.position = pos;
            widget.transform.scale = Vector3.one * scale;
        }

        static void MoveAndScaleToWorldPosition(VisualElement element, Vector3 worldPosition, Vector2 worldSize)
        {
            Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPosition, worldSize, Camera.main);
            Vector2 layoutSize = element.layout.size;
            
            // Don't set scale to 0 or a negative number.
            Vector2 scale = layoutSize.x > 0 && layoutSize.y > 0 ? rect.size / layoutSize : Vector2.one * 1e-5f;

            element.transform.position = rect.position;
            element.transform.scale = new Vector3(scale.x, scale.y, 1);
        }

        void Remove(Placeable _)
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (_healthModelPlaceable != null)
            {
                _healthModelPlaceable.HealthChanged -= viewModel.CurrentHealth;
                _healthModelPlaceable.OnDie -= Remove;
            }
            viewModel.CurrentHealth.OnValueChanged -= OnHealthChanged;
        }
    }
}