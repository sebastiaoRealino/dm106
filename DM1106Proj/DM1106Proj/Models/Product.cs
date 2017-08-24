using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DM1106Proj.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O	campo nome é obrigatório")]
        public string name { get; set; }

        public string description { get; set; }

        public string color { get; set; }

        [Required(ErrorMessage = "O	campo modelo é obrigatório")]
        public string model { get; set; }


        [Required]
        [StringLength(8, ErrorMessage = "O	tamanho	máximo do código e caracteres")]
        public string code { get; set; }

        [Range(10, 999, ErrorMessage = "O	preço deverá ser entre 10 e	999.")]
        public decimal preco { get; set; }

        public float weight { get; set; }

        public float height { get; set; }

        public float width { get; set; }

        public float lenght { get; set; }

        public float diameter { get; set; }

        public string urlImage { get; set; }

        [StringLength(80, ErrorMessage = "O	tamanho	máximo	da	url	é 80 caracteres")]
        public string Url { get; set; }
    }
}