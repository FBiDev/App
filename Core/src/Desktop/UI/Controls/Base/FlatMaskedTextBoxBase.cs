using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public class FlatMaskedTextBoxBase : MaskedTextBox
    {
        private static readonly PropertyInfo MaxLengthProperty = typeof(TextBoxBase).GetProperty("MaxLength", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        private static readonly MethodInfo MaxLengthMethod = MaxLengthProperty.GetSetMethod();
        private static readonly FieldInfo MaxLengthField = typeof(TextBoxBase).GetField("maxLength", BindingFlags.Instance | BindingFlags.NonPublic);

        public FlatMaskedTextBoxBase()
        {
            GotFocus += FlatMaskedTextBoxBase_GotFocus;
            LostFocus += FlatMaskedTextBoxBase_LostFocus;
            TextChanged += FlatMaskedTextBoxBase_TextChanged;

            MxLength = 50;
        }

        public string LastText { get; set; }

        public bool TxtChanged { get; set; }

        [DefaultValue(50)]
        public int MxLength { get; set; }

        public override int MaxLength
        {
            get { return base.MaxLength; }
            set { }
        }

        public void SetMaxLength(int value)
        {
            if (IsHandleCreated)
            {
                MaxLengthMethod.Invoke(this, new object[] { value });
                MaxLengthField.SetValue(this, value);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            SetMaxLength(MxLength);
        }

        private void FlatMaskedTextBoxBase_LostFocus(object sender, EventArgs e)
        {
            TxtChanged = false;
        }

        private void FlatMaskedTextBoxBase_GotFocus(object sender, EventArgs e)
        {
            TxtChanged = false;
        }

        private void FlatMaskedTextBoxBase_TextChanged(object sender, EventArgs e)
        {
            TxtChanged = false;

            if (Text.Length > MaxLength)
            {
                Text = LastText;
                SelectionStart = Text.Length;
            }

            if (Text != LastText && Focused)
            {
                LastText = Text;
                TxtChanged = true;
            }
        }
    }
}