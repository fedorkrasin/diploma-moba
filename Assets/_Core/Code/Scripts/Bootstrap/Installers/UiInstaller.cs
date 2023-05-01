﻿using Core.UI.Presenters;
using Core.UI.Views.Impl;
using Core.Util.Extensions;
using Zenject;

namespace Core.Installers.Bootstrap
{
    public class UiInstaller : Installer<UiInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindActors<CharacterSelectionView, CharacterSelectionPresenter>();
            Container.BindActors<PlayerControllerView, PlayerControllerPresenter>();
            Container.BindActors<NetworkControllerView, NetworkControllerPresenter>();
        }
    }
}