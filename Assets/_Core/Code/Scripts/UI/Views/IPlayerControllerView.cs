﻿using System;
using Core.UI.Components.Controller;
using Core.UI.ViewManagement.Actors;
using UnityEngine;

namespace Core.UI.Views
{
    public interface IPlayerControllerView : IView
    {
        bool IsMoving { get; }
        Vector2 MovementValue { get; }
        CooldownButton SpellButton { get; }
        CooldownButton UltimateButton { get; }
        
        event Action AttackButtonClicked;
        event Action SpellButtonClicked;
        event Action UltimateButtonClicked;
    }
}