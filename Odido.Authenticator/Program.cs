using TMobile.Api;
using TMobile.Api.Models;
internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.Title = "Odido authenticator";
        TMobileApi api = new TMobileApi(TmobileApiOptions.OdidoApiOptions);

        Console.WriteLine("Login at: {0}", api.GetLoginUrl());
        Console.WriteLine("-----");
        Console.WriteLine("After logging in with 2fa u will be redirected to a blank page. Copy the new url");
        Console.WriteLine("It should look somehting like: https://www.odido.nl/loginappresult?token=XXXXXXXX");

        Console.Write("Paste the new url: ");
        var refreshToken = api.GetRefreshToken(Console.ReadLine());
        Console.WriteLine("Refresh token (one time use): {0}", refreshToken);

        Console.WriteLine("Do you want to request a OAuth (Authentication token)? [Y/n]");

        var answer = Console.ReadKey();

        Console.WriteLine("Do you want to request a OAuth (Authentication token)? [Y/n]");
        if (IsYesResponse())
        {
            var oauthToken = await api.CreateOAuthToken(refreshToken);
            if (string.IsNullOrEmpty(oauthToken))
            {
                Console.WriteLine("Something went wrong.. try again");
                return;
            }
            Console.WriteLine("OAuth token: {0}", oauthToken);
            Console.WriteLine("WARNING! THIS IS A PASSWORD! KEEP THIS TOKEN SAFE.");
        }
    }

    private static bool IsYesResponse()
    {
        bool isValidInput = false;
        ConsoleKey answer;

        do
        {
            answer = Console.ReadKey().Key;

            if (answer == ConsoleKey.Enter)
            {
                answer = ConsoleKey.Y;  // Set default to 'Y' when pressing enter
                isValidInput = true;
            }
            else if (answer == ConsoleKey.Y || answer == ConsoleKey.N)
            {
                isValidInput = true;
            }
            else
            {
                Console.WriteLine("\nInvalid input. Please press 'Y' for yes, 'N' for no, or Enter for default (Yes).");
            }

        } while (!isValidInput);

        return answer == ConsoleKey.Y;
    }
}