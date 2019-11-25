using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WandhiHelper.ControlExtension
{
    public static class DataGridViewExtension
    {
        /// <summary>
        /// 自动设置列宽，并冻结指定列
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="indexs">冻结列索引</param>
        public static void AutoReSize(this DataGridView dgv, IList<int> indexs = null)
        {
            var width = 0;
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                //将每一列都调整为自动适应模式
                dgv.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                //记录整个DataGridView的宽度
                width += dgv.Columns[i].Width;
            }
            //判断调整后的宽度与原来设定的宽度的关系，如果是调整后的宽度大于原来设定的宽度，
            //则将DataGridView的列自动调整模式设置为显示的列即可，
            //如果是小于原来设定的宽度，将模式改为填充。
            if (width > dgv.Size.Width)
            {
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            else
            {
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            if (indexs != null)
            {
                //冻结某列 从左开始 0，1，2
                foreach (var item in indexs)
                {
                    dgv.Columns[item].Frozen = true;
                }
            }

        }
    }
}
