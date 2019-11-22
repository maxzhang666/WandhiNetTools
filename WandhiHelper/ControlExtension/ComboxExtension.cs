using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WandhiHelper.ControlExtension
{
    public static class ComboxExtension
    {
        private static LabelValueObject defaultItemObj = new LabelValueObject() { Value = "N/A" };
        /// <summary>
        /// 下拉框填充
        /// </summary>
        /// <param name="cbx"></param>
        /// <param name="items"></param>
        /// <param name="selectedItemValue"></param>
        /// <param name="defaultItem"></param>
        public static void FillComboBoxItems(this ComboBox cbx, IList<LabelValueObject> items, string selectedItemValue = null, string defaultItem = "选择...")
        {
            defaultItemObj.Label = defaultItem;
            cbx.Items.Clear();            
            cbx.Items.Add(defaultItemObj);
            cbx.SelectedItem = defaultItemObj;
            if (items != null)
            {
                foreach (var item in items)
                {
                    cbx.Items.Add(item);
                    if (item.IsDefault)
                    {
                        cbx.SelectedValue = item;
                    }
                    else if (!string.IsNullOrWhiteSpace(selectedItemValue) && (string.Compare(item.Value, selectedItemValue, true) == 0 || string.Compare(item.Label, selectedItemValue, true) == 0))
                    {
                        cbx.SelectedValue = item;
                    }
                }
            }
        }
        /// <summary>
        /// 获取下拉框值
        /// </summary>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static string GetComBoxValue(this ComboBox editor)
        {
            if (editor.SelectedItem == null)
            {
                return string.Empty;
            }
            LabelValueObject obj = editor.SelectedItem as LabelValueObject;
            if (obj == null || !obj.IsValid)
            {
                return string.Empty;
            }
            return obj.Value;
        }
        /// <summary>
        /// 获取下拉框文本
        /// </summary>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static string GetComboBoxText(this ComboBox editor)
        {
            if (editor.SelectedItem == null)
            {
                return string.Empty;
            }
            if (editor.SelectedItem is string)
            {
                return editor.SelectedItem.ToString();
            }
            LabelValueObject obj = editor.SelectedItem as LabelValueObject;
            if (obj == null || !obj.IsValid)
            {
                return string.Empty;
            }
            return obj.Label;
        }
    }



    public class LabelValueObject : IConvertible
    {
        public string Label { get; set; }
        public string Label1 { get; set; }
        public string Label2 { get; set; }
        public string Label3 { get; set; }

        public string Value { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }

        public bool IsDefault { get; set; }

        public object Tag { get; set; }

        public override string ToString()
        {
            return Label;
        }
        /// <summary>
        /// 如果Value值不是默认填充的 N/A 则为true 表示该项是有效的数据项，否则返回false
        /// </summary>
        public bool IsValid
        {
            get
            {
                return string.Compare("N/A", Value, StringComparison.OrdinalIgnoreCase) != 0;
            }
        }
        #region IConvertible
        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(Value);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(Value);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(Value);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(Value);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(Value);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(Value);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(Value);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(Value);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(Value);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(Value);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(Value);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(Value);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(Value);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(Value);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return Value;
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(Value, conversionType);
        }

        #endregion

    }
}
