using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    [Serializable]
    public abstract class RecordedValue<T>
    {
        [Serializable]
        public class KeyVal<KeyType, ValueType>
        {
            public KeyType Key;
            public ValueType Value;

            public KeyVal() { }
            public KeyVal(KeyType key, ValueType value)
            {
                this.Key = key;
                this.Value = value;
            }
        }

        [ShowInInspector] protected List<KeyVal<object, T>> dataList = new();

        public RecordedValue() { }
        public RecordedValue(T value)
        {
            dataList.Add(new(null, value));
            SetValue();
        }

        [ShowInInspector, ReadOnly] public T Value { get; protected set; } = default;

        protected abstract T SetValue();

        public virtual T Reset(T value = default)
        {
            dataList.Clear();
            return Update(value);
        }

        public virtual T Update(T value, object obj = null, Func<T, T> condition = null)
        {
            value = condition == null ? value : condition.Invoke(Value);
            dataList.Add(new(obj, value));
            return OnRefreshValue();
        }

        public virtual T Remove(object obj)
        {
            dataList.RemoveAll(x => x.Key == obj);
            return OnRefreshValue();
        }

        protected T OnRefreshValue()
        {
            Value = SetValue();
            OnUpdateValue?.Invoke(Value);
            OnUpdateValueVoid?.Invoke();
            return Value;
        }

        public event Action<T> OnUpdateValue;
        public event Action OnUpdateValueVoid;
    }

    [Serializable]
    public class RecordedFloat : RecordedValue<float>
    {
        protected override float SetValue() => dataList.Select(x => x.Value)?.Sum() ?? 0;
    }

    [Serializable]
    public class RecordedSheild : RecordedFloat
    {
        public override float Update(float value, object obj = null, Func<float, float> condition = null)
        {
            if (value < 0)
            {
                foreach (var data in dataList)
                {
                    var sheildValue = Mathf.Max(data.Value + value, 0);
                    value = data.Value + value;
                    data.Value = sheildValue;
                    if (value >= 0)
                        break;
                }
                return OnRefreshValue();
            }
            else
                return base.Update(value, obj, condition);
        }
    }

    [Serializable]
    public class RecordedFloatClamped : RecordedFloat
    {
        Func<float> minGetter;
        Func<float> maxGetter;

        public float Reset(Func<float> minGetter, Func<float> maxGetter, float value = 0)
        {
            this.minGetter = minGetter;
            this.maxGetter = maxGetter;
            return base.Reset(value);
        }

        public override float Update(float value, object obj = null, Func<float, float> condition = null)
            => base.Update(Mathf.Clamp(value, (minGetter?.Invoke() ?? 0) - Value, (maxGetter?.Invoke() ?? 0) - Value ), obj, condition);
    }
}
