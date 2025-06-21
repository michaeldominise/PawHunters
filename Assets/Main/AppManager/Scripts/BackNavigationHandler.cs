using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace LabHavenInteractive.PawHunters
{
    public class BackNavigationHandler : SingletonMonoBehaviour<BackNavigationHandler>
    {
        public enum BackHandlerMode { DoNothing, Execute, ExecuteAndRemoveSameKey }

        [SerializeField] InputActionReference backInput;
        [ShowInInspector] static List<KeyValuePair<object, Action>> backActionList = new();

        public static void Add(object key, Action backAction, BackHandlerMode backHandlerMode)
        {
            switch (backHandlerMode)
            {
                case BackHandlerMode.DoNothing:
                    return;
                case BackHandlerMode.ExecuteAndRemoveSameKey:
                    RemoveDuplicateKeyHirarchy(key);
                    break;
            }
            backActionList.Add(new(key, backAction));
        }

        static Action RemoveDuplicateKeyHirarchy(object key)
        {
            if (backActionList.Count == 0)
                return null;

            var index = backActionList.FindIndex(x => x.Key == key);
            if (index < 0)
                return null;

            var backNavigation = backActionList[index];
            backActionList.RemoveRange(index, backActionList.Count - index);

            return backNavigation.Value;
        }

        private void Start() => backInput.action.performed += OnBackClicked;

        private void OnBackClicked(InputAction.CallbackContext callbackContext)
        {
            if (backActionList.Count == 0)
                return;

            RemoveDuplicateKeyHirarchy(backActionList[^1].Key)?.Invoke();
        }

        private void OnDestroy()
        {
            backInput.action.performed -= OnBackClicked;
            backActionList.Clear();
        }
    }
}
