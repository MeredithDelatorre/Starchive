using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ArrowLauncher : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private float _speed = 45f;

    [Header("Visual Effects")]
    [SerializeField] private GameObject _trailSystem;

    private Rigidbody _rigidBody;
    private bool _inAir = false;
    private XRPullInteractable _pullInteractable;

    private void Awake()
    {
        InitializeComponents();
        SetPhysics(false);
    }

    private void InitializeComponents()
    {
        _rigidBody = GetComponent<Rigidbody>();
        if (_rigidBody == null)
        {
            Debug.LogError($"Rigidbody component not found on Arrow {gameObject.name}");
        }
    }

    public void Initialize(XRPullInteractable pullInteractable)
    {
        _pullInteractable = pullInteractable;
        _pullInteractable.PullActionReleased += Release;
    }

    private void OnDestroy()
    {
        if (_pullInteractable != null)
        {
            _pullInteractable.PullActionReleased -= Release;
        }
    }

    private void Release(float value) {
        // Unsubscribe so we don’t fire twice
        if (_pullInteractable != null)
            _pullInteractable.PullActionReleased -= Release;

        // Detach the arrow from the bow
        transform.parent = null;

        _inAir = true;

        /* ---- physics setup ---- */
        _rigidBody.isKinematic = false;   // allow collisions
        _rigidBody.useGravity = false;   // no arc

        /* ---- straight‑line launch ---- */
        _rigidBody.linearVelocity = transform.forward * value * _speed;

        StartCoroutine(RotateWithVelocity());   // keep arrow pointing along its path
        _trailSystem.SetActive(true);           // enable trail / VFX
    }


    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (_inAir)
        {
            if (_rigidBody != null && _rigidBody.linearVelocity.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(_rigidBody.linearVelocity, transform.up);
            }
            yield return null;
        }
    }

    public void StopFlight()
    {
        _inAir = false;
        SetPhysics(false);
        _trailSystem.SetActive(false);
    }

    private void SetPhysics(bool usePhysics)
    {
        if (_rigidBody != null)
        {
            _rigidBody.useGravity = usePhysics;
            _rigidBody.isKinematic = !usePhysics;
        }
    }
}