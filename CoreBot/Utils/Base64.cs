namespace CoreBot.Utils;

internal class Base64
{
    public static string EncodeToBase64(string texto)
    {
        try
        {
            byte[] textoAsBytes = Encoding.ASCII.GetBytes(texto);
            string resultado = System.Convert.ToBase64String(textoAsBytes);
            return resultado;
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    public static string DecodeFrom64(string dados)
    {
        try
        {
            byte[] dadosAsBytes = System.Convert.FromBase64String(dados);
            string resultado = System.Text.ASCIIEncoding.ASCII.GetString(dadosAsBytes);
            return resultado;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
