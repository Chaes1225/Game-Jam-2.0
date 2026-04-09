using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Bullet))]
public class ScreenBounceBullet : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private int maxBounces = 3;
    [SerializeField] private float slowdownRate = 20f;
    [SerializeField] private float stopSpeedThreshold = 0.15f;
    [SerializeField] private float pickupRadius = 1.1f;

    private Bullet bullet;
    private Camera mainCam;
    private SpriteRenderer bulletSprite;
    private Transform pickupTarget;
    private Action<ScreenBounceBullet> onPickedUp;
    private int bounceCount;
    private bool isLosingMomentum;
    private bool canBePickedUp;

    private void Awake()
    {
        bullet = GetComponent<Bullet>();
        bulletSprite = GetComponent<SpriteRenderer>();
        mainCam = Camera.main;
    }

    public void Fire(Transform newPickupTarget, Vector2 direction, Action<ScreenBounceBullet> pickedUpCallback)
    {
        pickupTarget = newPickupTarget;
        onPickedUp = pickedUpCallback;
        bounceCount = 0;
        isLosingMomentum = false;
        canBePickedUp = false;

        if (mainCam == null)
        {
            mainCam = Camera.main;
        }

        bullet.Launch(direction);
        RotateToVelocity(bullet.Rigidbody.linearVelocity);
    }

    private void Update()
    {
        if (!canBePickedUp)
        {
            return;
        }

        TryPickup();
    }

    private void FixedUpdate()
    {
        if (canBePickedUp)
        {
            return;
        }

        if (isLosingMomentum)
        {
            SlowDownUntilStopped();
            return;
        }

        HandleScreenBounce();
    }

    private void HandleScreenBounce()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
            if (mainCam == null)
            {
                return;
            }
        }

        Vector2 velocity = bullet.Rigidbody.linearVelocity;
        if (velocity.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        Vector3 bottomLeft = mainCam.ViewportToWorldPoint(Vector3.zero);
        Vector3 topRight = mainCam.ViewportToWorldPoint(Vector3.one);
        Vector2 extents = GetSpriteExtents();
        Vector3 currentPosition = transform.position;
        bool bounced = false;

        if (currentPosition.x <= bottomLeft.x + extents.x && velocity.x < 0f)
        {
            currentPosition.x = bottomLeft.x + extents.x;
            velocity.x = -velocity.x;
            bounced = true;
        }
        else if (currentPosition.x >= topRight.x - extents.x && velocity.x > 0f)
        {
            currentPosition.x = topRight.x - extents.x;
            velocity.x = -velocity.x;
            bounced = true;
        }

        if (currentPosition.y <= bottomLeft.y + extents.y && velocity.y < 0f)
        {
            currentPosition.y = bottomLeft.y + extents.y;
            velocity.y = -velocity.y;
            bounced = true;
        }
        else if (currentPosition.y >= topRight.y - extents.y && velocity.y > 0f)
        {
            currentPosition.y = topRight.y - extents.y;
            velocity.y = -velocity.y;
            bounced = true;
        }

        if (!bounced)
        {
            return;
        }

        transform.position = currentPosition;
        bounceCount++;
        RotateToVelocity(velocity);

        if (bounceCount >= maxBounces)
        {
            bullet.Rigidbody.linearVelocity = velocity;
            isLosingMomentum = true;
            return;
        }

        bullet.Rigidbody.linearVelocity = velocity;
    }

    private void SlowDownUntilStopped()
    {
        Vector2 currentVelocity = bullet.Rigidbody.linearVelocity;
        if (currentVelocity.sqrMagnitude <= stopSpeedThreshold * stopSpeedThreshold)
        {
            bullet.Stop();
            isLosingMomentum = false;
            canBePickedUp = true;
            return;
        }

        Vector2 slowedVelocity = Vector2.MoveTowards(
            currentVelocity,
            Vector2.zero,
            slowdownRate * Time.fixedDeltaTime);

        bullet.Rigidbody.linearVelocity = slowedVelocity;
        RotateToVelocity(slowedVelocity);
    }

    private void TryPickup()
    {
        if (pickupTarget == null)
        {
            return;
        }

        float pickupRadiusSqr = pickupRadius * pickupRadius;
        Vector2 pickupOffset = pickupTarget.position - transform.position;
        if (pickupOffset.sqrMagnitude > pickupRadiusSqr)
        {
            return;
        }

        onPickedUp?.Invoke(this);
        Destroy(gameObject);
    }

    private Vector2 GetSpriteExtents()
    {
        if (bulletSprite == null)
        {
            return Vector2.zero;
        }

        return bulletSprite.bounds.extents;
    }

    private void RotateToVelocity(Vector2 velocity)
    {
        if (velocity.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
