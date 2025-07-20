using Models.DTO;
using Models.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UIRequests
{
    public class UIItem
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        public required string Name { get; set; }

        public string? TechnicalDescription { get; set; }

        public DateOnly AcquisitionDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public AcquisitionType? AcquisitionType { get; set; }

        public string PurchaseValue { get; set; }

        public string? PurchaseStore { get; set; }

        public decimal? ResaleValue { get; set; }

        public ItemSituation? Situation { get; set; }

        public string? Comment { get; set; }

        public CategoryDTO? Category { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? WithdrawalDate { get; set; }

        public string? Image1 { get; set; }

        public string? Image2 { get; set; }
    }
}
