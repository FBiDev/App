using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public class FlatMaskedTextBoxBase : MaskedTextBox
    {
        public string LastText { get; set; }
        public bool TxtChanged { get; set; }

        [DefaultValue(50)]
        public int MxLength { get; set; }

        public override int MaxLength
        {
            get { return base.MaxLength; }
            set { }
        }

        public FlatMaskedTextBoxBase()
        {
            GotFocus += FlatMaskedTextBoxBase_GotFocus;
            LostFocus += FlatMaskedTextBoxBase_LostFocus;
            TextChanged += FlatMaskedTextBoxBase_TextChanged;

            MxLength = 50;
        }

        void FlatMaskedTextBoxBase_LostFocus(object sender, EventArgs e)
        {
            TxtChanged = false;
        }

        void FlatMaskedTextBoxBase_GotFocus(object sender, EventArgs e)
        {
            TxtChanged = false;
        }

        void FlatMaskedTextBoxBase_TextChanged(object sender, EventArgs e)
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

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            SetMaxLength(MxLength);
        }

        public void SetMaxLength(int value)
        {
            if (IsHandleCreated)
            {
                MaxLengthProperty_Set.Invoke(this, new object[] { value });
                MaxLengthField.SetValue(this, value);
            }
        }

        private static readonly PropertyInfo MaxLengthProperty = typeof(TextBoxBase).GetProperty("MaxLength", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        private static readonly MethodInfo MaxLengthProperty_Set = MaxLengthProperty.GetSetMethod();
        private static readonly FieldInfo MaxLengthField = typeof(TextBoxBase).GetField("maxLength", BindingFlags.Instance | BindingFlags.NonPublic);
    }
}