using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Model.Entity
{
    public class Custom_ListL<T> : List<T>
    {
        public int RowCount { get; set; }
    }
}
