
using Managers;
using UnityEngine;
using UnityEngine.Audio;

namespace EventCallbacks
{
    

    #region In Game Events

    public class OnDebrisDestroyed : Event<OnDebrisDestroyed>
    {
        
    }
    public class OnPlayerDamage : Event<OnPlayerDamage>
    {
        
    }


    public class OnGameOver : Event<OnGameOver>
    {
        
    }
    
    
    public class OnPlayerHealthChanged : Event<OnPlayerHealthChanged>
    {
        public float newHealth;

        public OnPlayerHealthChanged(float newHealth)
        {
            this.newHealth = newHealth;
        }
    }
    
    
    public class OnPlayerDeath : Event<OnPlayerDeath>
    {
        
    }
    
    public class OnEnemyDeath : Event<OnEnemyDeath>
    {
        public GameObject EnemyObject;

        public OnEnemyDeath(GameObject enemyObject)
        {
            EnemyObject = enemyObject;
        }
    }

    

    public class OnScoreChanged : Event<OnScoreChanged>
    {
        public int Score;

        public OnScoreChanged(int score)
        {
            Score = score;
        }
    }

    public class OnScoreGained : Event<OnScoreGained>
    {
        public int Value;

        public OnScoreGained(int value)
        {
            Value = value;
        }
    }
    
    public class OnScoreLost : Event<OnScoreLost>
    {
        public int Value;

        public OnScoreLost(int value)
        {
            Value = value;
        }
    }

  
    
    #endregion

    #region General Events

    public class OnMenuLoaded : Event<OnMenuLoaded>
    {
        
    }
    
    #endregion

    #region Time Events 

    public class TimeScaleEvent : Event<TimeScaleEvent>
    {
        public TimeScaleProperties TimeScaleProperty;

        public TimeScaleEvent(float timeScale, float duration, bool lerp, float lerpSpeed, bool infinite)
        {
            TimeScaleProperty.TimeScale = timeScale;
            TimeScaleProperty.Duration = duration;
            TimeScaleProperty.Lerp = lerp;
            TimeScaleProperty.LerpSpeed = lerpSpeed;
            TimeScaleProperty.Infinite = infinite;
        }        
        
    }

    public class FixedTimeScaleEvent : Event<FixedTimeScaleEvent>
    {
        public TimeScaleProperties TimeScaleProperty;

        public FixedTimeScaleEvent(float timeScale, float duration, bool lerp, float lerpSpeed, bool infinite)
        {
            TimeScaleProperty.TimeScale = timeScale;
            TimeScaleProperty.Duration = duration;
            TimeScaleProperty.Lerp = lerp;
            TimeScaleProperty.LerpSpeed = lerpSpeed;
            TimeScaleProperty.Infinite = infinite;
        }        
        
    }

    public class OnTimePaused : Event<OnTimePaused>
    {
        
    }

    public class OnTimeResume : Event<OnTimeResume>
    {
        
    }
    
    #endregion


    #region Sound Events

    public class SoundEvent : Event<SoundEvent>
    {
        public AudioClip Clip;
        public float Volume;
        public float Pitch;

        public SoundEvent(AudioClip clip)
        {
            Clip = clip;
            Volume = 1f;
            Pitch = 1f;
        }
    }

    #endregion
    
}
