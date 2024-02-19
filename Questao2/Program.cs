using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team1, int year)
    {
        int goals = 0;
        using (HttpClient client = new HttpClient())
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team1}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                JObject data = JObject.Parse(content);

                JArray matches = (JArray)data["data"];

                foreach (var matche in matches)
                    goals += int.Parse(matche["team1goals"].ToString());
            }
            else
                Console.WriteLine($"Error: {response.StatusCode}");

            return goals;
        }
    }

}