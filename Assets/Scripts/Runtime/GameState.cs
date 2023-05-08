using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using ZSBB.Level;

namespace ZSBB {
    [CreateAssetMenu]
    sealed class GameState : ScriptableObject {
        [SerializeField]
        AnimalManager animals;

        public bool hasWon => animals.outCount == 0;

        public bool hasLost => currentStability == 0;

        [SerializeField, ReadOnly]
        float timeLimit = -1;
        [SerializeField, ReadOnly]
        float timer = 0;

        public void StartTimer(float timeLimit) {
            this.timeLimit = timeLimit;
            timer = timeLimit + 0.99f;
        }

        public void StopTimer() {
            timeLimit = -1;
        }

        public void UpdateTimer(float deltaTime) {
            timer -= deltaTime;
        }

        float timerHealth {
            get {
                if (timeLimit < 0) {
                    return 1;
                }

                return Mathf.Clamp01(timer / timeLimit);
            }
        }
        string timerText {
            get {
                if (timeLimit < 0) {
                    return "--:--";
                }

                if (timer < 0) {
                    return "00:00";
                }

                var time = TimeSpan.FromSeconds(Mathf.Floor(timer));
                return time.ToString(@"mm\:ss");
            }
        }

        int currentStability {
            get {
                if (!Tower.instance) {
                    return 1;
                }

                return Tower.instance.currentSegmentCount;
            }
        }
        int maxStability {
            get {
                if (!Tower.instance) {
                    return 1;
                }

                return Tower.instance.maxSegmentCount;
            }
        }

        float stabilityHealth {
            get {
                return (float)currentStability / maxStability;
            }
        }
        string stabilityText {
            get {
                return string.Empty.PadLeft(currentStability, 'x').Replace("x", "🧱");
            }
        }

        float animalsHealth {
            get {
                return (float)animals.inCount / animals.totalCount;
            }
        }
        string animalsText {
            get {
                return $"{animals.inCount}/{animals.totalCount}";
            }
        }

        public IEnumerable<(string key, string value)> variables {
            get {
                yield return ("$version", Application.version);
                yield return ("$productName", Application.productName);
                yield return ("$companyName", Application.companyName);

                yield return ("$animals", AddColor(animalsText, animalsHealth));
                yield return ("$stability", AddColor(stabilityText, stabilityHealth));
                yield return ("$timer", AddColor(timerText, timerHealth));
            }
        }

        static string AddColor(string text, float health) {
            var color = Color.Lerp(Color.red, Color.green, health);
            string html = ColorUtility.ToHtmlStringRGB(color);
            return $"<color=#{html}>{text}</color>";
        }
    }
}
