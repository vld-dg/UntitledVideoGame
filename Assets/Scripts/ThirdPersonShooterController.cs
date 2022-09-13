using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    private StarterAssetsInputs _starterAssetsInputs;
    private ThirdPersonController _thirdPersonController;
    private Animator _animator;

    private void Awake()
    {
        _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }
        
        if (_starterAssetsInputs.aim)
        {
            _virtualCamera.gameObject.SetActive(true);
            _thirdPersonController.SetRotateOnMove(false);
            
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            _virtualCamera.gameObject.SetActive(false);
            _thirdPersonController.SetRotateOnMove(true);
        }
        
        if (_starterAssetsInputs.attack)
        {
            _animator.SetLayerWeight(1,Mathf.Lerp(_animator.GetLayerWeight(1),1f,Time.deltaTime * 10f));
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(pfBulletProjectile, spawnBulletPosition.position,Quaternion.LookRotation(aimDir,Vector3.up));
            _starterAssetsInputs.attack = false;
        }
        else
        {
            _animator.SetLayerWeight(1,Mathf.Lerp(_animator.GetLayerWeight(1),0f,Time.deltaTime * 10f));
        }
    }
}