using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PickUp : Interactable
{
    // Types that the player will still able to interact with when holding an object
    [SerializeField] private string[] _typeNamesThatCanInteract;
    private List<Type> _typesThatCanInteract;

    private bool _isHolding = false;

    private Rigidbody _rb;
    private Collider[] _colliders;
    private Player _interactor;
    private Color _previousColor;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _colliders = GetComponents<Collider>();
        _typesThatCanInteract = new List<Type>();

        foreach (string typeName in _typeNamesThatCanInteract)
        {
            Type type = Assembly.GetExecutingAssembly().GetType(typeName);

            if (type != null)
                _typesThatCanInteract.Add(type);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isHolding && Input.GetKeyDown(KeyCode.E) && !_interactor.HasInteractableInRange && !_interactor.IsExamining)
        {
            StartCoroutine(Drop());
        }
    }

    public override void Interact(Player interactor)
    {
        StartCoroutine(Pickup(interactor));
    }

    private IEnumerator Pickup(Player holder)
    {
        yield return new WaitForEndOfFrame();

        _interactor = interactor;
        _isHolding = true;
        _previousColor = _outlineColor;

        //_interactor.CanInteract = false;
        RemoveInteractTypes();
        AddInteractTypes();
        _interactor.IsHoldingItem = true;

        _rb.useGravity = false;
        _rb.angularVelocity = Vector3.zero;
        _rb.velocity = Vector3.zero;

        foreach (Collider collider in _colliders)
        {
            collider.enabled = false;
        }

        transform.parent = interactor.HoldPoint.transform;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        Unhighlight();
    }

    private IEnumerator Drop()
    {
        _isHolding = false;
        _rb.useGravity = true;
        transform.parent = null;

        foreach (Collider collider in _colliders)
        {
            collider.enabled = true;
        }

        _outlineColor = _previousColor;
        _interactor.IsHoldingItem = false;

        yield return new WaitForEndOfFrame();

        //_interactor.CanInteract = true;
        RemoveInteractTypes();
        _interactor.CanInteractTypes.Add(typeof(Interactable));
        _interactor = null;
    }

    private void AddInteractTypes()
    {
        foreach (Type type in _typesThatCanInteract)
        {
            _interactor.CanInteractTypes.Add(type);
        }
    }

    private void RemoveInteractTypes()
    {
        _interactor.CanInteractTypes.Clear();
    }
}
