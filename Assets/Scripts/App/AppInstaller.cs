using App.Timer;
using App.Timer.States;
using App.Timer.Windows;
using UnityEngine;
using Zenject;

namespace App
{
    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AppState>().AsSingle();
            Container.Bind<AppWindowsState>().AsSingle();
        }
    }
}