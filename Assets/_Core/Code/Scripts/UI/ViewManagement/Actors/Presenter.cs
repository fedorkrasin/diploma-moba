using System;
using Zenject;

namespace Core.UI.ViewManagement.Actors
{
    public class Presenter<TView> : IInitializable, IDisposable where TView : IView
    {
        protected TView View { get; }

        protected Presenter(TView screen)
        {
            View = screen ?? throw new ArgumentNullException(nameof(screen));
        }

        public virtual void Initialize()
        {
            
        }

        public virtual void Dispose()
        {
            
        }
    }
}