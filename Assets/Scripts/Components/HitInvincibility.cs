using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitInvincibility : MonoBehaviour
{
    public float Duration = 1.0f;

    private SpriteRenderer _spriteRenderer;

    private bool _active;
    
    void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _active = false;
    }

    public bool IsActive()
    {
        return _active;
    }

    public void StartInvincibility()
    {
        _active = true;
        StartCoroutine(HitInvincibilityRoutine());
    }

    IEnumerator HitInvincibilityRoutine()
    {
        float started = Time.realtimeSinceStartup;
        
        while (started + Duration > Time.realtimeSinceStartup)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled; // flicker
            yield return new WaitForSeconds(0.1f);
        }

        _spriteRenderer.enabled = true;
        _active = false;
    }
}
