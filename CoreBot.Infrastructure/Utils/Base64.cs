namespace CoreBot.Infrastructure.Utils;

public static class Base64
{
    public static string EncodeBase64(this string value)
    {
        var valueBytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(valueBytes);
    }

    public static string DecodeBase64(this string value)
    {
        var valueBytes = System.Convert.FromBase64String(value).Where(c => c is not 0).ToArray();
        return Encoding.Default.GetString(valueBytes);
    }
}
