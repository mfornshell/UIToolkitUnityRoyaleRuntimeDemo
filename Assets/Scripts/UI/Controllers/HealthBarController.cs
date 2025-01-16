using UnityEngine;
using UnityEngine.UIElements;

namespace UnityRoyale
{
    public class HealthBarController : UIController<HealthBarVM>
    {
        ThinkingPlaceable _healthModelPlaceable; // TODO : replace this with a proper healthModel
        VisualElement _healthBarElement;

        [SerializeField] VisualTreeAsset _healthBarAsset;

        [SerializeField] private Vector2 worldSize = new Vector2(1f, 1f);

        [SerializeField] Vector3 anchorPosition;
        [SerializeField] Transform transformToFollow;

        public void Initialize(ThinkingPlaceable p, Vector3 anchor, float hitPoints, Color color)
        {
            viewModel.Initialize(hitPoints, color);

            _healthModelPlaceable = p;
            _healthModelPlaceable.HealthChanged += viewModel.CurrentHealth;
            _healthModelPlaceable.OnDie += Remove;

            viewModel.CurrentHealth.OnValueChanged += OnHealthChanged;

            anchorPosition = anchor;
            transformToFollow = p.transform;
        }

        public void SetupVisualElement(UIDocument uiDocument)
        {
            view = uiDocument;
            view.rootVisualElement.Add(_healthBarElement);
        }

        void OnEnable()
        {
            viewModel = ScriptableObject.CreateInstance<HealthBarVM>();
            var tree = _healthBarAsset.Instantiate();
            _healthBarElement = tree.Q<HealthBarElement>();
            _healthBarElement.dataSource = viewModel;
        }

        void OnHealthChanged(float newHealth) => viewModel.IsVisible.Value = newHealth < viewModel.OriginalHealth;

        private void LateUpdate()
        {
            if (viewModel.IsVisible && transformToFollow != null)
            {
                MoveAndScaleToWorldPosition(_healthBarElement, transformToFollow.position + anchorPosition, worldSize);
            }
        }

        static void MoveAndScaleToWorldPosition(VisualElement element, Vector3 worldPosition, Vector2 worldSize)
        {
            if (element.panel == null) return;
            Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPosition, worldSize, Camera.main);
            Vector2 layoutSize = element.layout.size;

            // Don't set scale to 0 or a negative number.
            Vector2 scale = layoutSize.x > 0 && layoutSize.y > 0 ? rect.size / layoutSize : Vector2.one * 1e-5f;

            element.transform.position = rect.position;
            element.transform.scale = new Vector3(scale.x, scale.y, 1);
        }

        void Remove(Placeable _) => Destroy(gameObject);

        private void OnDestroy()
        {
            if (_healthModelPlaceable != null)
            {
                _healthModelPlaceable.HealthChanged -= viewModel.CurrentHealth;
                _healthModelPlaceable.OnDie -= Remove;
            }
            viewModel.CurrentHealth.OnValueChanged -= OnHealthChanged;
            _healthBarElement.RemoveFromHierarchy();
        }
    }
}
