using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace UnityRoyale
{
    [UxmlElement]
    public partial class CardElement : VisualElement
    {
        CardData _cardData;
        public CardData cardData => _cardData;

        [UxmlAttribute, CreateProperty] public int Index { get; set; }
        Translate _position = new();

        public void Init(CardData cardData)
        {
            _cardData = cardData;
        }
        
        public void ChangeActiveState(bool isActive)
        {
            this.style.opacity = (isActive) ? .05f : 1f;
        }

        public void MoveAndScaleIntoPosition(int distance, Vector2 position)
        {
            AnimatedMoveTo(position, distance);
            Scale(1f);
        }
        
        public void Scale(float ratio)
        {
            transform.scale = Vector3.one * ratio;
        }
        public void AnimatedScale(float endScale, float tweenDuration)
        {
            experimental.animation.Scale(endScale, Mathf.RoundToInt(tweenDuration * 1000)).Ease(Easing.OutQuad);
        }

        public void MoveTo(Vector2 screenPosition)
        {
            _position = new Translate(screenPosition.x, screenPosition.y);
            style.translate = _position;
        }
        public ValueAnimation<Vector3> AnimatedMoveTo(Vector2 endPosition, float distance)
        {
            return experimental.animation.Position(new Vector3(endPosition.x, endPosition.y, transform.position.z),
                Mathf.RoundToInt(GetDuration(distance) * 1000)).Ease(Easing.OutQuad);
        }
        public void Translate(Vector2 screenPositionDelta)
        {
            transform.position += (Vector3) screenPositionDelta;
        }

        public void SetAsLastSibling()
        {
            BringToFront();
        }

        public void Delete()
        {
            RemoveFromHierarchy();
        }

        public void ResetPosition() => style.translate = _position;

        public float GetDuration(float distance) => .2f + (.05f * distance);
    }
}