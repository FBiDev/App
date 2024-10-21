using System;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Core.Desktop;

namespace App.Cohab
{
    public enum TratamentoTipo
    {
        Limpar,
        Habilitar,
        Desabilitar
    }

    public static class Funcoes
    {
        public enum CriptografiaTipo
        {
            Simples,
            Delphi,
            Cohab
        }

        public static string CriptografarSenha(string aTexto, bool aEncriptar = true)
        {
            string lRetorno = string.Empty;

            string criptOld = "áéíóúãõÃÕÁÉÍÓÚ1234567890 -=!@#$%¨&*()_+¹²³£¢¬§QWERTYUIOP`{qwertyuiop´[ªASDFGHJKLÇ^}asdfghjklç~]º|ZXCVBNM<>:?\\zxcvbnm,.;/°";
            string criptNew = "/A?B°C²D:E;F>G.áH,I<JÇÁºK]L~mÐ^nÕ}o¬p|q\\ZÉr{s`ét¹u´ãv[wªx=y+z§çÍ-aõ_b2c)d4e(f6gí*hÓ³i8j&Ãk0l¨M1N%O3PÚ$Q5R£Só#T7U@V9W!Xú¢Y";

            if (!aEncriptar)
            {
                string aux = criptOld;
                criptOld = criptNew;
                criptNew = aux;
            }

            var senhaEncriptadaReverse = aTexto.Reverse();

            foreach (var c in senhaEncriptadaReverse)
            {
                var index = criptOld.IndexOf(c);
                lRetorno += criptNew[index];
            }

            return lRetorno;
        }

        public static string NomeDoComputador()
        {
            var lNome = System.Net.Dns.GetHostName();
            return lNome;
        }

        public static string UsuarioRede()
        {
            string lUsuario;
            try
            {
                lUsuario = WindowsIdentity.GetCurrent().Name;
            }
            catch (Exception)
            {
                lUsuario = string.Empty;
            }

            return lUsuario;
        }

        public static string WhoPrinted(string aUsuario, string aLotacao, string aSistema)
        {
            return aUsuario.ToLower() + "/" + NomeDoComputador() + " - " + CriptografarWhoPrinted(UsuarioRede() + ":" + NomeDoComputador() + ":" + aUsuario.ToLower() + ":" + aLotacao + ":" + aSistema, true);
        }

        public static string CriptografarWhoPrinted(string aTexto, bool aEncriptar = true)
        {
            string lRetorno = string.Empty;

            var lMyKey = "DTI-SEDE";
            var lHashMD5 = new MD5CryptoServiceProvider();
            var lDes = new TripleDESCryptoServiceProvider
            {
                Key = lHashMD5.ComputeHash(Encoding.ASCII.GetBytes(lMyKey)),
                Mode = CipherMode.ECB
            };

            if (aEncriptar)
            {
                var lDesdencrypt = lDes.CreateEncryptor();
                var myASCIIEncoding = new ASCIIEncoding();
                var lBuffer = Encoding.ASCII.GetBytes(aTexto);
                lRetorno = Convert.ToBase64String(lDesdencrypt.TransformFinalBlock(lBuffer, 0, lBuffer.Length));
            }
            else
            {
                var lDesdencrypt = lDes.CreateDecryptor();
                var lBuffer = Convert.FromBase64String(aTexto);
                lRetorno = Encoding.ASCII.GetString(lDesdencrypt.TransformFinalBlock(lBuffer, 0, lBuffer.Length));
            }

            return lRetorno;
        }

        public static async Task<string> MostrarLotacao(string aLogin)
        {
            // Selecionar os dados da última lotação e carregar as propriedades
            var lSQL = "SELECT CASE WHEN Diretoria.Diretoria_Sigla IS NOT NULL THEN " +
                        "RTRIM(Diretoria.Diretoria_Sigla) ELSE '' END + " +
                        "CASE WHEN Departamento.Departamento_Sigla IS NOT NULL THEN '-' + " +
                        "RTRIM(Departamento.Departamento_Sigla) ELSE '' END + " +
                        "CASE WHEN Setor.Setor_Sigla IS NOT NULL THEN '-' + RTRIM(Setor.Setor_Sigla) ELSE '' END AS Lotacao " +
                        "FROM Diretoria " +
                        "RIGHT OUTER JOIN Lotacao ON Diretoria.Diretoria_Id = Lotacao.Lotacao_DiretoriaId " +
                        "LEFT OUTER JOIN Departamento ON Lotacao.Lotacao_DepartamentoId = Departamento.Departamento_Id " +
                        "LEFT OUTER JOIN Setor ON Lotacao.Lotacao_SetorId = Setor.Setor_Id " +
                        "WHERE Lotacao.Lotacao_DataFim IS NULL AND " +
                        "Lotacao.Lotacao_UsuarioLogin = '" + aLogin + "'";
            var result = await BancoCOHAB.ExecutarSelectString(lSQL);
            return result;
        }

        public static void TratarFormulario(Form aFormulario, TratamentoTipo aTratamento)
        {
            bool lHabilitar = !(aTratamento == TratamentoTipo.Desabilitar);

            foreach (Control lControle in aFormulario.Controls)
            {
                if (aTratamento == TratamentoTipo.Limpar)
                {
                    TratarFormularioLimparContainer(lControle);
                }
                else
                {
                    TratarFormularioHabilitarContainer(lControle, lHabilitar);
                }
            }
        }

        private static void TratarFormularioLimparContainer(Control aContainer)
        {
            if (aContainer is GroupBox ||
               aContainer is FlowLayoutPanel ||
               aContainer is TableLayoutPanel ||
               aContainer is Panel)
            {
                foreach (Control lControleA in aContainer.Controls)
                {
                    TratarFormularioLimparContainer(lControleA);
                }
            }
            else if (aContainer is SplitContainer || aContainer is TabControl)
            {
                foreach (Control lControleA in aContainer.Controls)
                {
                    if (lControleA is SplitterPanel || lControleA is TabPage)
                    {
                        foreach (Control lControleB in lControleA.Controls)
                        {
                            TratarFormularioLimparContainer(lControleB);
                        }
                    }
                }
            }
            else
            {
                TratarFormularioLimpar(aContainer, aContainer);
            }
        }

        private static void TratarFormularioHabilitarContainer(Control aContainer, bool aHabilitar)
        {
            if (aContainer is TabControl || aContainer is SplitContainer)
            {
                aContainer.Enabled = true;
                foreach (Control lControleA in aContainer.Controls)
                {
                    if (lControleA is TabPage || lControleA is SplitterPanel)
                    {
                        lControleA.Enabled = true;
                        foreach (Control lControleB in lControleA.Controls)
                        {
                            if (lControleB.GetType().ToString() == "MyComponents.uBlinkLabel" ||
                               lControleB is LinkLabel ||
                               lControleB is Label)
                            {
                                lControleB.Enabled = true;
                            }
                            else
                            {
                                TratarFormularioHabilitarContainer(lControleB, aHabilitar);
                            }

                            if (aHabilitar == false)
                            {
                                TratarFormularioBranco(lControleB);
                            }
                        }
                    }
                }
            }
            else if (aContainer is GroupBox || aContainer is FlowLayoutPanel ||
                     aContainer is TableLayoutPanel || aContainer is Panel)
            {
                foreach (Control lControleA in aContainer.Controls)
                {
                    if (lControleA.GetType().ToString() == "MyComponents.uBlinkLabel" ||
                       lControleA is LinkLabel || lControleA is Label)
                    {
                        lControleA.Enabled = true;
                    }
                    else
                    {
                        TratarFormularioHabilitarContainer(lControleA, aHabilitar);
                    }

                    if (aHabilitar == false)
                    {
                        TratarFormularioBranco(lControleA);
                    }
                }
            }
            else
            {
                TratarFormularioHabilitar(aContainer, aHabilitar);
            }
        }

        private static void TratarFormularioLimpar(Control aTipoControle, Control aControle)
        {
            switch (aTipoControle.GetType().Name)
            {
                case "RichTextBox": aControle.Text = string.Empty;
                    break;
                case "TextBox": aControle.Text = string.Empty;
                    break;
                case "MaskedTextBox": aControle.Text = string.Empty;
                    break;
                case "ComboBox":
                    aControle.Text = string.Empty;
                    ((ComboBox)aControle).SelectedIndex = -1;
                    break;
                case "CheckBox": ((CheckBox)aControle).Checked = false;
                    break;
                case "DateTimePicker": ((DateTimePicker)aControle).Value = DateTime.Today;
                    break;
                case "ListView": ((ListView)aControle).Clear();
                    break;
                case "ListBox": ((ListBox)aControle).SelectedIndex = -1;
                    break;
                case "uDecimalBox": aControle.Text = string.Empty;
                    break;
                case "uMaskedBox": aControle.Text = string.Empty;
                    break;
                case "uTextBox": aControle.Text = string.Empty;
                    break;
                case "uTextBoxAutoSelect": aControle.Text = string.Empty;
                    break;
                case "uComboBoxAutoComplete":
                    aControle.Text = string.Empty;
                    ((ComboBox)aControle).SelectedIndex = -1;
                    break;
            }

            // Custom Controls
            switch (aTipoControle.GetType().BaseType.Name)
            {
                case "FlatTextBox": ((FlatTextBox)aControle).Text = string.Empty;
                    break;
                case "FlatComboBox": ((FlatComboBox)aControle).ResetIndex();
                    break;
            }
        }

        private static void TratarFormularioBranco(Control aControle)
        {
            if (!(aControle is CheckBox) &&
                !(aControle is Label) &&
                !(aControle is LinkLabel) &&
                !(aControle is SplitContainer) &&
                !(aControle is GroupBox) &&
                !(aControle is RadioButton) &&
                aControle.GetType().ToString() != "MyComponents.uBlinkLabel")
            {
                aControle.BackColor = Color.White;
            }
        }

        private static void TratarFormularioHabilitar(Control aControle, bool aHabilitar)
        {
            if (aControle.GetType().ToString() != "MyComponents.uToolbar" &&
               aControle.GetType().ToString() != "MyComponents.uBlinkLabel" &&
               aControle.GetType().ToString() != "MyComponents.uStatusBar" &&
               !(aControle is LinkLabel) &&
               !(aControle is ToolStrip) &&
               !(aControle is StatusStrip) &&
               !(aControle is Label))
            {  // And aControle.GetType.ToString <> "System.Windows.Forms.Button" Then
                aControle.Enabled = aHabilitar;

                if (aHabilitar == false)
                {
                    TratarFormularioBranco(aControle);
                }
            }
        }
    }
}