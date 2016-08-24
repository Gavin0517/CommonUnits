using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Units
{
    /// <summary>
    /// 查询排序字段
    /// </summary>
    public class SortInfo
    {
        public SortDir direction;
        public string field;

        public SortInfo()
        {
            this.field = "ID";
            this.direction = SortDir.DESC;
        }

        public SortInfo(string info)
            : this()
        {
            try
            {
                string[] strArray = info.Split(new char[] { ':' });
                this.field = strArray[0];
                this.direction = (strArray[1].ToLower() == "asc") ? SortDir.ASC : SortDir.DESC;
            }
            catch
            {
            }
        }

        public SortInfo(string sort, string dir)
            : this()
        {
            try
            {
                this.field = sort;
                if (dir.ToLower().Trim() == "asc")
                {
                    this.direction = SortDir.ASC;
                }
                else
                {
                    this.direction = SortDir.DESC;
                }
            }
            catch
            {
            }
        }

        public SortInfo(string sort, SortDir dir)
            : this()
        {
            try
            {
                this.field = sort;
                this.direction = dir;
            }
            catch
            {
            }
        }
    }
}
