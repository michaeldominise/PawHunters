using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHaven.PawHunters
{
    public class StatusTextUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI label;
        [SerializeField] Image iconImage;

        AnimationCurve AnimationTextCurve = AppSettings_Battle.Instance?.constantValues.progressUpdateAnimationCurve ?? default;
        AnimationCurve AnimationTransformCurve = AppSettings_Battle.Instance?.constantValues.bounceAnimationCurve ?? default;
        float UpdateDuration => AppSettings_Battle.Instance.constantValues.progressUpdateDuration;
        float TargetYPosition => AppSettings_Battle.Instance.constantValues.statusTextUITargetYPosition;
        float TargetScale => AppSettings_Battle.Instance.constantValues.statusTextUITargetScale;
        float LifeDuration => AppSettings_Battle.Instance.constantValues.statusTextUILifeDuration;
        Camera WorldCamera => Camera.main;
        RectTransform RectParent => transform.parent as RectTransform;

        public void Init(Vector3 worldPosition, float randomAdditionalDistance, Color colorLabel, float value, AppSettings_Battle.Type type = AppSettings_Battle.Type.None)
        {
            Init(worldPosition, randomAdditionalDistance, colorLabel, type);
            GradualChangeValue.Execute(0, value, UpdateDuration, OnProgressTextUpdate, AnimationTextCurve);
        }

        public void Init(Vector3 worldPosition, float randomAdditionalDistance, Color colorLabel, string text, AppSettings_Battle.Type type = AppSettings_Battle.Type.None)
        {
            Init(worldPosition, randomAdditionalDistance, colorLabel, type);
            label.text = text;
            GradualChangeValue.Execute(0, 1, UpdateDuration, OnProgressTransformUpdate, AnimationTransformCurve);
        }

        public void Init(Vector3 worldPosition, float randomAdditionalDistance, Color colorLabel, AppSettings_Battle.Type type = AppSettings_Battle.Type.None)
        {
            var viewportPoint = WorldCamera.WorldToViewportPoint(worldPosition);
            var halfScreenSize = new Vector3(RectParent.rect.width, RectParent.rect.height) * 0.5f;
            transform.localPosition = new Vector3(Mathf.LerpUnclamped(-halfScreenSize.x, halfScreenSize.x, viewportPoint.x), Mathf.LerpUnclamped(-halfScreenSize.y, halfScreenSize.y, viewportPoint.y)) + randomAdditionalDistance * (Vector3)Random.insideUnitCircle;

            label.color = colorLabel;
            var sprite = AppSettings_Battle.Instance.iconSprite.GetSprite(type);
            iconImage.sprite = sprite;
            iconImage.gameObject.SetActive(sprite);

            Kill();
        }

        protected virtual void OnProgressTextUpdate(GradualChangeValue.Status status)
        { 
            label.text = $"{(status.CurrentValue > 0 ? "+" : "")}{status.CurrentValue.Format()}";
            OnProgressTransformUpdate(status);
        }

        protected virtual void OnProgressTransformUpdate(GradualChangeValue.Status status)
        {
            label.transform.localPosition = Mathf.LerpUnclamped(0, TargetYPosition, status.progress) * Vector3.up;
            label.transform.localScale = Mathf.LerpUnclamped(0, TargetScale, status.progress) * Vector3.one;
        }

        async void Kill()
        {
            await Task.Delay((int)(LifeDuration * 1000));
            gameObject.SetActive(false);
        }
    }
}
