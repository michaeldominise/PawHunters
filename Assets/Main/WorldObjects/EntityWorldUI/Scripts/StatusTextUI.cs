using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PawHunters
{
    public class StatusTextUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI label;
        [SerializeField] Image iconImage;

        AnimationCurve animationCurve = GlobalSettings.Instance?.gameSettings.progressUpdateAnimationCurve ?? default;
        float UpdateDuration => GlobalSettings.Instance.gameSettings.progressUpdateDuration;
        float TargetYPosition => GlobalSettings.Instance.gameSettings.statusTextUITargetYPosition;
        float TargetScale => GlobalSettings.Instance.gameSettings.statusTextUITargetScale;
        float LifeDuration => GlobalSettings.Instance.gameSettings.statusTextUILifeDuration;
        Camera WorldCamera => Camera.main;
        RectTransform RectParent => transform.parent as RectTransform;

        public void Init(Vector3 worldPosition, float randomAdditionalDistance, Color colorLabel, float value, CommonIconSettings.Icon icon = CommonIconSettings.Icon.None)
        {
            Init(worldPosition, randomAdditionalDistance, colorLabel);
            GradualChangeValue.Execute(0, value, UpdateDuration, OnProgressUpdate, animationCurve);
        }

        public void Init(Vector3 worldPosition, float randomAdditionalDistance, Color colorLabel, string text, CommonIconSettings.Icon icon = CommonIconSettings.Icon.None)
        {
            Init(worldPosition, randomAdditionalDistance, colorLabel);
            label.text = text;
        }

        public void Init(Vector3 worldPosition, float randomAdditionalDistance, Color colorLabel, CommonIconSettings.Icon icon = CommonIconSettings.Icon.None)
        {
            var viewportPoint = WorldCamera.WorldToViewportPoint(worldPosition);
            var halfScreenSize = new Vector3(RectParent.rect.width, RectParent.rect.height) * 0.5f;
            transform.localPosition = new Vector3(Mathf.LerpUnclamped(-halfScreenSize.x, halfScreenSize.x, viewportPoint.x), Mathf.LerpUnclamped(-halfScreenSize.y, halfScreenSize.y, viewportPoint.y)) + randomAdditionalDistance * (Vector3)Random.insideUnitCircle;

            label.color = colorLabel;
            var sprite = StatusTextUISpawner.Instance.GetIcon(icon);
            iconImage.sprite = sprite;
            iconImage.gameObject.SetActive(sprite);

            Kill();
        }

        protected virtual void OnProgressUpdate(GradualChangeValue.Status status)
        {
            label.text = $"{(status.CurrentValue > 0 ? "+" : "")}{status.CurrentValue.Format()}";
            label.transform.localPosition = Mathf.Lerp(0, TargetYPosition, status.progress) * Vector3.up;
            label.transform.localScale = Mathf.Lerp(0, TargetScale, status.progress) * Vector3.one;
        }

        async void Kill()
        {
            await Task.Delay((int)(LifeDuration * 1000));
            gameObject.SetActive(false);
        }
    }
}
