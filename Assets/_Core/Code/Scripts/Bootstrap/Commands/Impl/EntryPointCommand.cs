using UnityEngine;
using Zenject;

namespace Core.Bootstrap.Commands.Impl
{
    public class EntryPointCommand : IEntryPointCommand, IInitializable
    {
        public void Initialize()
        {
            Execute();
        }
        
        public void Execute()
        {
            Application.targetFrameRate = 120;
            
            Debug.Log(this);
        }
    }
}