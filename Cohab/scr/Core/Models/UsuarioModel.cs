using System.ComponentModel;

namespace App.Cohab
{
    public class UsuarioModel
    {
        public UsuarioModel()
        {
            Login = Nome = Ramal = Matricula = string.Empty;
        }

        public string Login { get; set; }

        [Style(Width = 270)]
        public string Nome { get; set; }

        [Style(Align = ColumnAlign.Center)]
        public string Matricula { get; set; }

        [Style(Align = ColumnAlign.Center)]
        public string Ramal { get; set; }

        [Display(AutoGenerateField = false)]
        public string Email { get; set; }

        public bool Ativo { get; set; }

        [Display(AutoGenerateField = false)]
        public int TipoColaborador { get; set; }

        public bool Chefia { get; set; }

        public bool Visivel { get; set; }

        [Display(AutoGenerateField = false)]
        public string Notes { get; set; }

        public override string ToString()
        {
            return Nome + " - " + Login;
        }
    }
}