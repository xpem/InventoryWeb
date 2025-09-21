using Models.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class ItemConfigsApiResp
    {
        public List<AcquisitionType> AcquisitionTypes { get; set; }

        public List<ItemSituation> ItemSituations { get; set; }

        public List<CategoryDTO> Categories { get; set; }

        /// <summary>
        /// for auto complete in the purchase store field
        /// </summary>
        public List<string> LastPurchaseStores { get; set; }
    }
}
