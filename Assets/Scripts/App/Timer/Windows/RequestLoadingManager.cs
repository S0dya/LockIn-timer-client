using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using UniRx;
using Zenject;

namespace App.Timer.Windows
{
    public class RequestLoadingManager : IInitializable
    {
        [Inject(Id = "App")] private WindowsManager _windowsManager;
        
        private readonly HashSet<IDisposable> _loadingQueue = new();
        
        public void Initialize()
        {
        }
        
        public IDisposable AddLoading()
        {
            var loadingToken = new LoadingToken(this);
            _loadingQueue.Add(loadingToken);
            
            if (_loadingQueue.Count == 1 && !_windowsManager.IsOpen<RequestLoadingWindow>())
            {
                _windowsManager.Open<RequestLoadingWindow>().Forget();
            }
            
            return loadingToken;
        }
        
        private void RemoveLoading(IDisposable loadingToken)
        {
            _loadingQueue.Remove(loadingToken);
            
            if (_loadingQueue.Count == 0)
            {
                _windowsManager.Close<RequestLoadingWindow>().Forget();
            }
        }
        
        private class LoadingToken : IDisposable
        {
            private readonly RequestLoadingManager _manager;
            private bool _disposed;
            
            public LoadingToken(RequestLoadingManager manager)
            {
                _manager = manager;
            }
            
            public void Dispose()
            {
                if (!_disposed)
                {
                    _disposed = true;
                    _manager.RemoveLoading(this);
                }
            }
        }
    }
}
