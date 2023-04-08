using EventCallbacks;
using System.Collections;
using UnityEngine;

public class HealthController : MonoBehaviour, IHealth
{
    public bool shouldUpdateHealthBar;

    [SerializeField] private float _maximumHealth = 20;


    [SerializeField] private float _currentHP;

    [SerializeField] private Color _startColor;

    [SerializeField] private Color _endColor;

    [SerializeField] private float _blendDuration;

    [SerializeField] private SpriteRenderer _sprite;

    private IDeath _deathHandler;

    private HealthChangedEvent _healthChangedEvent = new HealthChangedEvent();

    protected virtual void Awake()
    {
        _deathHandler = GetComponent<IDeath>();
    }


    public void Initialize(float initialHealth)
    {
        _maximumHealth = initialHealth;
        _currentHP = _maximumHealth;
    }

    public void Enrage(float newMaximum) 
    {
        _currentHP = Mathf.RoundToInt(_currentHP * (newMaximum / _maximumHealth)); 
        _maximumHealth = Mathf.RoundToInt(newMaximum);
        _healthChangedEvent.RaiseLocal();
    }

    public void AddListener(GameEvent.EventDelegate<GameEvent> listener)
    {
        _healthChangedEvent.AddLocalListener(listener);
    }

    public void RemoveListener(GameEvent.EventDelegate<GameEvent> listener)
    {
        _healthChangedEvent.RemoveLocalListener(listener);
    }

    protected void ChangeHealth(float amount, bool isCriticalHit = false, FloatingColourType colourType = FloatingColourType.Generic)
    {
        if (_currentHP > 0)
        {
            if (isCriticalHit)
            {
                GenerateCombatText(amount, FloatingColourType.levelUp, true);
            }
            else
            {
                GenerateCombatText(amount, colourType);
            }
        }

        _currentHP += amount;

        if (_currentHP > _maximumHealth)
        {
            _currentHP = _maximumHealth;
        }
        if (_currentHP <= 0)
        {
            _deathHandler.StartDeath();
        }

        _healthChangedEvent.ChangeValue = amount;
        _healthChangedEvent.RaiseLocal();

        FlashColour();
    }

    protected virtual void GenerateCombatText(float amount, FloatingColourType colourType, bool isCriticalHit = false)
    {
        if (amount > 0)
        {
            FloatingCombatTextController.Instance.CreateFloatingCombatText("+" + amount, transform, FloatingColourType.Heal, false);
        }
        else if (amount < 0)
        {
            if (isCriticalHit)
            {

                FloatingCombatTextController.Instance.CreateFloatingCombatText(Mathf.Abs(amount).ToString(), transform, colourType, false, 0.9f, 0.8f);
            }
            else
            {
                FloatingCombatTextController.Instance.CreateFloatingCombatText(Mathf.Abs(amount).ToString(), transform, colourType, false);
            }
        }
    }

    public void Kill()
    {
        _currentHP = 0;

        FlashColour();


        _deathHandler.CompleteDeath();
    }

    public float CurrentHealth()
    {
        return _currentHP;
    }

    public float GetMaxHP()
    {
        return _maximumHealth;
    }

    private void FlashColour()
    {
        _sprite.color = _endColor;
        StopCoroutine(BlendColors());
        StartCoroutine(BlendColors());
    }

    private IEnumerator BlendColors()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _blendDuration)
        {
            _sprite.color = Color.Lerp(_startColor, _endColor, elapsedTime / _blendDuration);
            elapsedTime += Time.deltaTime;
            yield return null;

            if (_deathHandler.IsDead())
            {
                _blendDuration = 0f;
                _sprite.color = _endColor;
            }
        }

        _sprite.color = _endColor;
    }

}
