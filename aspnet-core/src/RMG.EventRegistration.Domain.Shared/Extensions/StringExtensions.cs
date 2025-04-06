namespace RMG.EventRegistration.Extensions;

public static class StringExtensions
{
    public static bool IsEmpty(this string input)
    {
        return string.IsNullOrEmpty(input);
    }

    public static bool IsNotEmpty(this string input)
    {
        return !input.IsEmpty();
    }

}
