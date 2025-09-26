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
        [MaxLength(249, ErrorMessage = "O campo Nome deve ter no máximo 249 caracteres")]
        public required string Name { get; set; }

        [MaxLength(349, ErrorMessage = "O campo Nome deve ter no máximo 349 caracteres")]
        public string? TechnicalDescription { get; set; }

        public DateOnly AcquisitionDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public AcquisitionType? AcquisitionType { get; set; }

        // PurchaseValue agora é opcional e aceita 0
        [RegularExpression(@"^(0|([1-9]\d{0,7}(\.\d{1,2})?))$", ErrorMessage = "O campo Valor de Compra deve ser um número decimal positivo com até 8 dígitos inteiros e 2 casas decimais, podendo ser 0")]
        [StringLength(11, ErrorMessage = "O campo Valor de Compra deve ter no máximo 11 caracteres")]
        public string? PurchaseValue { get; set; }

        [MaxLength(99, ErrorMessage = "O campo Nome deve ter no máximo 99 caracteres")]
        public string? PurchaseStore { get; set; }

        [StringLength(11, ErrorMessage = "O campo Valor de Compra deve ter no máximo 11 caracteres")]
        public string? ResaleValue { get; set; }

        public ItemSituation? Situation { get; set; }

        [MaxLength(349, ErrorMessage = "O campo Nome deve ter no máximo 349 caracteres")]
        public string? Comment { get; set; }

        public CategoryDTO? Category { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateOnly WithdrawalDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public string? Image1Base64 { get; set; }

        public bool IsImage1Base64 { get; set; } = true;

        //public string? Image2 { get; set; }
    }
}
