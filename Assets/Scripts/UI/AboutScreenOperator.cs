using UnityEngine;
using UnityEngine.UIElements;

namespace UnityRoyale
{
    [UxmlElement]
    public partial class AboutScreenOperator : VisualElement
    {
        public void Animate()
        {
            var scrollView = this.Q<ScrollView>();
            scrollView.transform.position = new Vector2(0, 2000);
            scrollView.experimental.animation.Position(new Vector2(0, 0), 1000);
        }
    }
}
