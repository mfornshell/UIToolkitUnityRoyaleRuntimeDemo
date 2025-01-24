using UnityEngine;
using UnityEngine.UIElements;

namespace UnityRoyale
{
    public class PlayingAreaUI : UIController<ViewModel>
    {
        public PlayingAreaElement Element { get; private set; }

        void Start()
        {
        
        }

        public void Initialize(PlayingAreaElement element)
        {
            Element = element;
        }

    }
}
