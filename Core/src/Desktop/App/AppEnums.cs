namespace App.Core.Desktop
{
    public enum Align
    {
        None,
        Left,
        Center,
        Right,
        Top,
        Bottom
    }

    public enum Movimento
    {
        Nenhum,
        Consulta,
        Inclusão,
        Alteração,
        Exclusão
    }

    public enum MenuMode
    {
        Consulta,
        Inclusão,
        Edição
    }

    public enum LabelType
    {
        normal,
        primary,
        success,
        danger,
        custom
    }

    public enum PanelType
    {
        transparent,
        control,
        controlLight,
        controlDark,
        white,
        custom
    }

    public enum TextMask
    {
        None,
        CELULAR,
        CEP,
        CNPJ,
        CPF,
        DATA,
        DINHEIRO,
        HORA,
        NUMERO
    }

    public enum OSVersion
    {
        Unknown = 0,
        Windows_95 = 40,
        Windows_98 = 410,
        Windows_98SE = 411,
        Windows_ME = 490,
        Windows_NT3 = 300,
        Windows_NT4 = 400,
        Windows_2000 = 50,
        Windows_XP = 51,
        Windows_XP_x64 = 521,
        Windows_Server_2003 = 523,
        Windows_Vista = 601,
        Windows_Server_2008 = 603,
        Windows_7 = 611,
        Windows_Server_2008_R2 = 613,
        Windows_8 = 621,
        Windows_Server_2012 = 623,
        Windows_8_1 = 631,
        Windows_Server_2012_R2 = 633,
        Windows_10 = 1001,
        Windows_Server_2016 = 1003,
        Windows_11 = 1022000
    }

    public enum OSEdition
    {
        Professional = 256,
        Home = 512
    }
}