namespace App.Cohab
{
    public class SistemaModel
    {
        public SistemaModel()
        {
            Sigla = Nome = string.Empty;
        }

        public string Sigla { get; set; }

        public string Nome { get; set; }

        public override string ToString()
        {
            return Nome + " - " + Sigla;
        }
    }
}