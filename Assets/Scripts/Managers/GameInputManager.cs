using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool.Singleton;

public class GameInputManager :Singleton <GameInputManager>
{
    GameInputAction _gameInputAction;
    public float Horizontal => _gameInputAction.GameInput.Horizontal.ReadValue<float>(); 

    public float Vertical => _gameInputAction.GameInput.Vertical.ReadValue<float>();
    public bool Jump => _gameInputAction.GameInput.Jump.WasPressedThisFrame();
    public bool Dash => _gameInputAction.GameInput.Dash.triggered;
    public bool Attack => _gameInputAction.GameInput.Attack.IsPressed();
    public bool Ability => _gameInputAction.GameInput.Ability.WasPressedThisFrame();
    public bool callMenu => _gameInputAction.GameInput.callMenu.WasPressedThisFrame();

    public bool SwitchPlayer => _gameInputAction.GameInput.SwitchPlayer.WasPressedThisFrame();

    public bool TurnOn => _gameInputAction.GameInput.SwitchPlayer.WasPressedThisFrame();

    public bool GhostVision => _gameInputAction.GameInput.GhostVision.IsPressed();  

    protected override void Awake()
    {
        base.Awake();
        _gameInputAction ??= new GameInputAction();
    }
    private void OnEnable()
    {
        _gameInputAction.Enable();
    }
    private void OnDisable()
    {
        _gameInputAction.Disable();
    }
}
