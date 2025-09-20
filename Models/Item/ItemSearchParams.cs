using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Item
{
    public class ItemSearchParams(string? Name, int[]? Situations, ResultOrderBy? OrderBy)
    {
        public string? Name { get; set; } = Name;

        public int[]? Situations { get; set; } = Situations;

        public ResultOrderBy? OrderBy { get; set; } = OrderBy;
    }

    public enum ResultOrderBy
    {
        CreatedAt, Name, AcquisitionDate, UpdatedAt
    }
}
