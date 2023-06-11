public class ExceptionHandler
{
    public static void HandleException(Exception exception)
    {
        Console.WriteLine($"An exception occurred: {exception.Message}");
    }
}