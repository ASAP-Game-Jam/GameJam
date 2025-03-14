using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public enum ModifyType { Delete }

    public class UIModifyButton : MonoBehaviour
    {
        public ModifyType Type;
        public event EventHandler OnSelect;
        public event EventHandler OnCancel;
        public event EventHandler OnClick;

        private bool _select;

        private void Start()
        {
            var button = GetComponent<Button>();
            if (button)
                button.onClick.AddListener(() =>
                {
                    if (_select)
                        Cancel();
                    else
                        Select();

                    this.OnClick?.Invoke(this, GetArgs());
                });
        }

        public void Select()
        {
            _select = true;
            OnSelect?.Invoke(this, GetArgs());
            GetComponentInChildren<TMP_Text>().color = Color.red; 
        }
        public void Cancel()
        {
            _select = false;
            OnCancel?.Invoke(this, GetArgs());
            GetComponentInChildren<TMP_Text>().color = Color.white;
        }
        private EventArgs GetArgs()
        {
            return EventArgs.Empty;
        }
    }
}
