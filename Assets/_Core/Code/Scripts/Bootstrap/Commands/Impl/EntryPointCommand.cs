using System;
using Core.UI.ViewManagement;
using Core.UI.ViewManagement.Data;
using UnityEngine;
using Zenject;

namespace Core.Bootstrap.Commands.Impl
{
    public class EntryPointCommand : IEntryPointCommand, IInitializable
    {
        private readonly ViewManager _viewManager;

        public EntryPointCommand(
            ViewManager viewManager)
        {
            _viewManager = viewManager ?? throw new ArgumentNullException(nameof(viewManager));
        }
        
        public void Initialize()
        {
            Execute();
        }
        
        public void Execute()
        {
            Application.targetFrameRate = 120;
            
            _viewManager.OpenView(ViewId.NetworkController);
        }
    }
}