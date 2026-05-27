using System;
using System.Collections.Generic;
using App.Timer.Back.Models;
using Cysharp.Threading.Tasks;
using PT.Tools.Debugging;
using PT.Tools.Windows;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace App.Timer.Run.History
{
    public class RunHistoryWindow : WindowBase
    {
        [SerializeField] private Transform runHistoryViewsParent;
        [SerializeField] private RunHistoryView runHistoryViewPrefab;
        [SerializeField] private int defaultCapacity = 30;
        [Space] 
        [SerializeField] private GameObject simpleRotateLoadingObj;
        [Space]
        [SerializeField] private Button[] closeWindowButtons;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private float scrollLoadOffset = 0.01f;
        
        private readonly List<RunHistoryView> _activeRunHistoryViews = new();
        
        private ObjectPool<RunHistoryView> _runHistoryViewsPool;

        public event Action TryLoadMore;
        public event Action CloseWindow;
        
        private bool _isLoadingMore;

        private float _lastContentHeight;

        public void Init()
        {
            _runHistoryViewsPool = new ObjectPool<RunHistoryView>(
                () => Instantiate(runHistoryViewPrefab, runHistoryViewsParent),
                (view) => view.gameObject.SetActive(true),
                (view) => view.gameObject.SetActive(false),
                defaultCapacity: defaultCapacity);
            
            foreach (var closeWindowButton in closeWindowButtons) closeWindowButton.onClick.AddListener(() => CloseWindow?.Invoke());
        }

        private void Update()
        {
            if (_isLoadingMore) return;
            if (!scrollRect.content) return;

            var contentHeight = scrollRect.content.rect.height;
            var viewportHeight = scrollRect.viewport.rect.height;
            
            if (contentHeight <= viewportHeight) return;

            var currentBottomY = scrollRect.content.anchoredPosition.y + scrollRect.viewport.rect.height;

            if (currentBottomY > contentHeight + scrollLoadOffset && contentHeight > _lastContentHeight)
            {
                OnTryLoadMore(contentHeight);
            }
        }

        protected override async UniTask OnOpen()
        {
            await base.OnOpen();
            
            scrollRect.verticalNormalizedPosition = 1f;
        }
        
        public void ShowRunHistories(List<RunHistoryResponse> runHistories)
        {
            DebugManager.Log(DebugCategory.TimerRun, $"Showing {runHistories.Count} run histories");
            
            for (int i = 0; i < runHistories.Count; i++)
            {
                var runHistoryView = _runHistoryViewsPool.Get();
                _activeRunHistoryViews.Add(runHistoryView);
                
                runHistoryView.SetView(new (
                    runHistories[i].CompletedSessions, 
                    runHistories[i].PlannedSessionsAmount, 
                    runHistories[i].SessionDuration, 
                    runHistories[i].Description,
                    runHistories[i].RunStartTime,
                    runHistories[i].RunEndTime));

                runHistoryView.transform.SetAsLastSibling();
            }

            DebugManager.Log(DebugCategory.TimerRun, $"Run histories shown, stopping loading");
            
            StopLoading();
        }

        public void StopLoading()
        {
            simpleRotateLoadingObj.SetActive(false);
            _isLoadingMore = false;
        }

        public void WipeRunHistories()
        {
            DebugManager.Log(DebugCategory.TimerRun, "Wiping run histories");
            
            foreach (var runHistoryView in _activeRunHistoryViews)
                _runHistoryViewsPool.Release(runHistoryView);

            _activeRunHistoryViews.Clear();

            _isLoadingMore = false;
            _lastContentHeight = 0;
        }
        
        private void OnTryLoadMore(float contentHeight)
        {
            if (_isLoadingMore) return;
            
            DebugManager.Log(DebugCategory.TimerRun, "Attempting to load more run histories");
            
            _isLoadingMore = true;
            _lastContentHeight = contentHeight;
            TryLoadMore?.Invoke();
            simpleRotateLoadingObj.SetActive(true);
        }
    }
}