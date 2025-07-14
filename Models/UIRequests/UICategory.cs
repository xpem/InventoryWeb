using Models.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UIRequests
{
    public class UICategory
    {
        public int Id { get; set; }

        public required string Name { get; set; }

       public string BackgoundColor { get; set; }

        public string Color { get; set; }

        public bool HaveSubcategories { get; set; }

        public bool SystemDefault { get; set; }

        public List<SubCategoryDTO> SubCategories { get; set; }
    }
}
