using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Beneficiario
    /// </summary>
    public class BeneficiarioModel
    {
        public long Id { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        [MaxLength(14)]
        public string CPF { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }

        public bool isSaved { get; set; }

        public bool isDeleted { get; set; }

        public bool changed { get; set; }
    }
}