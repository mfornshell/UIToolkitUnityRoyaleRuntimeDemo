using UnityEngine;
using UnityEngine.UIElements;

namespace UnityRoyale
{
    public class HealthBarController : UIController<HealthBarVM>
    {
        ThinkingPlaceable _healthModelPlaceable; // TODO : replace this with a proper healthModel
        VisualElement _healthBarElement;
        Camera _camera;

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
            _camera = Camera.main;
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
                SetPositionAndScale(transformToFollow.position + anchorPosition);
            }
        }

        void SetPositionAndScale(Vector3 worldPos)
        {
            if (_healthBarElement?.panel == null) return;

            var rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(
                _healthBarElement.panel, worldPos, worldSize, _camera);

            _healthBarElement.style.translate = new Translate(rect.position.x, rect.position.y);

            var scale = rect.size / _healthBarElement.layout.size;
            _healthBarElement.style.scale = new Scale(scale);
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
