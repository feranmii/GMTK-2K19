using System;
using System.Collections.Generic;
using EventCallbacks;
using NaughtyAttributes;
using UnityEngine;

namespace Managers
{
    public class TimeManager : MonoBehaviour
    {
        public float NormalTimeScale = 1f;

        [ReadOnly] public float CurrentTimeScale = 1f;
        
        // The Time Scale the system is lerping towards
        [ReadOnly] public float TargetTimeScale = 1f;
        
        //If the timescale should be lerping
        [ReadOnly] public bool LerpTimeScale = true;
        
        //Lerping speed of the timescale towards its target
        [ReadOnly] public float LerpSpeed;

        protected Stack<TimeScaleProperties> _timeScaleProperties;
        protected TimeScaleProperties _currentTimeScaleProperty;
        
        private void Start()
        {
            TargetTimeScale = NormalTimeScale;
            _timeScaleProperties = new Stack<TimeScaleProperties>();
            
        }

        private void OnEnable()
        {
            TimeScaleEvent.RegisterListener(TimeScaleEventTrigger);
        }

        private void OnDisable()
        {
            TimeScaleEvent.UnregisterListener(TimeScaleEventTrigger);
        }

        
        public void TimeScaleEventTrigger(TimeScaleEvent tse)
        {
            //_currentTimeScaleProperty = tse.TimeScaleProperty;
            SetTimeScale(tse.TimeScaleProperty);
        }

        [Button()]
        public void TestButtonSlowDown()
        {
            TimeScaleEvent tse = new TimeScaleEvent(0.5f, 3f, true, 3f, false);
            tse.FireEvent();
        }

        
        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.F))
            {
                TestButtonSlowDown();
            }
            if (_timeScaleProperties.Count > 0)
            {
                _currentTimeScaleProperty = _timeScaleProperties.Peek();
                TargetTimeScale = _currentTimeScaleProperty.TimeScale;
                LerpSpeed = _currentTimeScaleProperty.LerpSpeed;
                LerpTimeScale = _currentTimeScaleProperty.Lerp;
                _currentTimeScaleProperty.Duration -= Time.unscaledDeltaTime;

                _timeScaleProperties.Pop();
                _timeScaleProperties.Push(_currentTimeScaleProperty);

                if (_currentTimeScaleProperty.Duration <= 0 && !_currentTimeScaleProperty.Infinite)
                {
                    UnFreeze();
                }

            }
            else
            {
                TargetTimeScale = NormalTimeScale;
            }
            
            //Apply the Timescale
            if (LerpTimeScale)
            {
                Time.timeScale = Mathf.Lerp(Time.timeScale, TargetTimeScale, Time.unscaledDeltaTime * LerpSpeed);
            }
            else
            {
                Time.timeScale = TargetTimeScale;
            }

            Time.fixedDeltaTime = Time.timeScale * .02f;
            CurrentTimeScale = Time.timeScale;
        }

        public void SetTimeScale(TimeScaleProperties timeScaleProperties)
        {
            _timeScaleProperties.Push(timeScaleProperties);
        }

        public void SetTimeScale(float newTimeScale)
        {
            _timeScaleProperties.Clear();
            Time.timeScale = newTimeScale;
        }

        private void UnFreeze()
        {
            if (_timeScaleProperties.Count > 0)
            {
                _timeScaleProperties.Pop();
            }

            else
            {
                ResetTimeScale();
            }
        }

        private void ResetTimeScale()
        {
            Time.timeScale = NormalTimeScale;
        }
    }
    
    
    
    public struct TimeScaleProperties
    {
        public float TimeScale;
        public float Duration;
        public float LerpSpeed;
        public bool Lerp;
        public bool Infinite;
        
    }
    
    
    
}