using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class CharacterInteract : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Header("Holding")]
    [SerializeField] Transform holdObjectRoot;
    [SerializeField] CharacterHoldObjectTransformDatas holdObjectTransformData;
    [SerializeField] float pickUpDuration = .5f;
    [SerializeField] float throwHoldingObjectForce = 2f;
    [SerializeField] Rig holdingRig;
    [SerializeField] int holdingAnimatorLayerIndex = 1;

    public PickableObject HoldingObject => holdingObject;

    protected virtual void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public float ThrowHoldingObjectForce => throwHoldingObjectForce;

    public virtual void Interact(InteractableObject interactableObject)
    {
        interactableObject.Interact(this);
    }

    protected PickableObject holdingObject;
    protected Tween pickUpTween;

    public virtual void PickUp(PickableObject pickableObject)
    {
        if (holdingObject != null)
        {
            DropHoldingObject();
        }
        holdingObject = pickableObject;
        holdingObject.transform.SetParent(holdObjectRoot, true);
        TransformData transformData = TransformData.Default;
        if (holdObjectTransformData != null)
            transformData = holdObjectTransformData.GetHeldObjectTransformData(pickableObject.ObjectNameKey);
        KillPickUpTween();
        Sequence sequence = DOTween.Sequence();
        sequence.Join(holdingObject.transform.DOLocalMove(transformData.position, pickUpDuration));
        sequence.Join(holdingObject.transform.DOLocalRotate(transformData.rotation, pickUpDuration));
        sequence.Join(
            DOVirtual.Float(0, 1, pickUpDuration, value =>
            {
                holdingRig.weight = value;
                animator.SetLayerWeight(holdingAnimatorLayerIndex, value);
            }));
        sequence.OnComplete(() => pickUpTween = null);
        pickUpTween = sequence;
    }

    public virtual void DropHoldingObject()
    {
        if (holdingObject == null) return;
        KillPickUpTween();
        pickUpTween = DOVirtual.Float(1, 0, pickUpDuration, value =>
        {
            holdingRig.weight = value;
            animator.SetLayerWeight(holdingAnimatorLayerIndex, value);
            pickUpTween = null;
        });
        holdingObject.transform.SetParent(null, true);
        holdingObject.Drop(this);
        holdingObject = null;
    }

    protected virtual void KillPickUpTween()
    {
        pickUpTween?.Kill();
    }
}
