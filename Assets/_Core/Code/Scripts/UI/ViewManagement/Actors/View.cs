using System;
using Core.Util.Misc;
using UnityEngine;
using Zenject;

namespace Core.UI.ViewManagement.Actors
{
    public class View : MonoBehaviour, IView, IInitializable, IDisposable
    {
        private readonly Disposables _disposables = new();

        public virtual void Initialize()
        {
            
        }

        public virtual void Dispose()
        {
            _disposables.Dispose();
        }
        
        // public IEnumerator PlayIn(Action callback = null)
        // {
        //     yield return PlayTransition(t => t.PlayIn(), callback);
        // }
        //
        // public IEnumerator PlayOut(Action callback = null)
        // {
        //     yield return PlayTransition(t => t.PlayOut(), callback);
        // }
        //
        // protected void AddDisposable(IDisposable disposable)
        // {
        //     _disposables.Add(disposable);
        // }
        //
        // private IEnumerator PlayTransition(Func<Transition, IEnumerator> action, Action callback)
        // {
        //     if (TryGetComponent<Transition>(out var transition))
        //     {
        //         yield return action(transition);
        //     }
        //
        //     callback?.Invoke();
        // }
    }
}