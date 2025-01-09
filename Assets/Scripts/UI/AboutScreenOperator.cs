using UnityEngine;
using UnityEngine.UIElements;

namespace UnityRoyale
{
    [UxmlElement]
    public partial class AboutScreenOperator : VisualElement
    {
        ScrollView m_ScrollView;

        public AboutScreenOperator()
        {
            RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        void OnGeometryChange(GeometryChangedEvent evt)
        {
            m_ScrollView = this.Q<ScrollView>();
            Animate();

            UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        public void Animate()
        {
            if (m_ScrollView == null)
                return;

            m_ScrollView.transform.position = new Vector2(0, 2000);
            m_ScrollView.experimental.animation.Position(new Vector2(0, 0), 1000);
        }
    }
}
