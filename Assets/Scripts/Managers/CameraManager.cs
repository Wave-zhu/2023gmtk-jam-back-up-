using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Tool.Singleton;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    public float _fallSpeedYDampingChangeThreshold = -15f;

    public bool IsLerpingYDamping {  get; private set; }// avoid multiple corutine


    private Coroutine _lerpYPanCorutine;
    private CinemachineFramingTransposer _framingTransposer;

    private float _normYPanAmount;

    protected override void Awake()
    {
        _framingTransposer = _allVirtualCameras[0].GetCinemachineComponent<CinemachineFramingTransposer>();
    }
    private void OnEnable()
    { 
        GameEventManager.MainInstance.AddEventListener<float>("LerpYDamping", LerpYDamping);
        GameEventManager.MainInstance.AddEventListener<float>("SwitchCamera",SwitchCamera);
    }
    private void OnDisable()
    {
        GameEventManager.MainInstance.RemoveEvent<float>("LerpYDamping", LerpYDamping);
        GameEventManager.MainInstance.RemoveEvent<float>("SwitchCamera", SwitchCamera);
    }
    private void SwitchCamera(float velocity)
    {
        if (velocity < _fallSpeedYDampingChangeThreshold)
        {
            _allVirtualCameras[0].Priority = 0;
            _allVirtualCameras[1].Priority = 1;
            _framingTransposer = _allVirtualCameras[1].GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        else
        {
            _allVirtualCameras[0].Priority = 1;
            _allVirtualCameras[1].Priority = 0;
            _framingTransposer = _allVirtualCameras[0].GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }
    #region Lerp the Y Damping
    private void LerpYDamping(float velocity)
    {
        if (velocity >= 0f&&!IsLerpingYDamping)
        {
            _lerpYPanCorutine = StartCoroutine(LerpYAction(false));
        }
        else if (velocity < _fallSpeedYDampingChangeThreshold && !IsLerpingYDamping)
        {
            _lerpYPanCorutine = StartCoroutine(LerpYAction(true));
        }       
    }
    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;
        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        if(isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;
        }
        else
        {
            endDampAmount = _normYPanAmount;
        }

        float elapsedTime = 0f;
        while (elapsedTime <_fallPanAmount)
        {
            elapsedTime += Time.deltaTime;
            float lerpedPanAmout = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / _fallYPanTime));
            _framingTransposer.m_YDamping = lerpedPanAmout;

            yield return null;
        }

        IsLerpingYDamping = false;
    }
    #endregion
    public void ResetFollowPoint(Transform T1,Transform T2)
    {
        _allVirtualCameras[0].Follow = T1;
        _allVirtualCameras[1].Follow = T2;
    }


}
