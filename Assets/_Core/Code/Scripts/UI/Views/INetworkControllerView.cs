using System;
using Core.UI.ViewManagement.Actors;

namespace Core.UI.Views
{
    public interface INetworkControllerView : IView
    {
        event Action ServerClicked;
        event Action HostClicked;
        event Action ClientClicked;
    }
}