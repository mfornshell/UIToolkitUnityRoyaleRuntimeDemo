using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIController<T> : MonoBehaviour where T : ViewModel
{
    [SerializeField] protected T viewModel;
    [SerializeField] protected UIDocument view;

    public virtual void Show()
    {
        view.sortingOrder = 1;
    }
    public virtual void Hide()
    {
        view.sortingOrder = -1;
    }
}
