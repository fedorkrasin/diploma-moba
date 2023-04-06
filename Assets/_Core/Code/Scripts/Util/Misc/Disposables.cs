using System;
using System.Collections.Generic;

namespace Core.Util.Misc
{
    public class Disposables
    {
        private readonly List<IDisposable> _disposables = new();

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable?.Dispose();
            }
        }

        public void Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }
    }
}