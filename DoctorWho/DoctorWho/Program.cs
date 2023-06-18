// See https://aka.ms/new-console-template for more information
using DoctorWho.Db;
using DoctorWho.Db.DTO;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace DoctorWho;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        DoctorWhoCoreDbContext dbContext = new DoctorWhoCoreDbContext();

        CallCompanionsFunction(dbContext, 1);
        CallEnemiesFunction(dbContext, 3);
        CallEpisodesView(dbContext);
        Console.WriteLine("-------------------------------------");
        CallSummariseEpisodesProcedure(dbContext);


         Console.ReadLine();


    }

    public static void CallCompanionsFunction(DoctorWhoCoreDbContext dbContext, int Id)
    {
        Console.Write("\nCompanions : ");
        var companions = dbContext.Companions.Select(c => dbContext.CallFnCompanions(Id)).FirstOrDefault();
        Console.WriteLine(companions);

    }

    public static void CallEnemiesFunction(DoctorWhoCoreDbContext dbContext, int Id)
    {
        Console.Write("\nEnemies : ");

        var enemies = dbContext.Enemies.Select(e => dbContext.CallFnEnemies(Id)).FirstOrDefault();
        Console.WriteLine(enemies);
    }

    public static void CallEpisodesView(DoctorWhoCoreDbContext dbContext)
    {
        Console.WriteLine("\nEpisodes View : ");


        var Query = dbContext.viewEpisodes.ToList();


        foreach (var item in Query)
        {

            Console.WriteLine($"EpisodeId {item.EpisodeId} :");
            Console.WriteLine(item.AuthorName);
            Console.WriteLine(item.Companions);
            Console.WriteLine(item.DoctorName);
            Console.WriteLine(item.Enemies);
            Console.WriteLine();


        };


    }
    public static void CallSummariseEpisodesProcedure(DoctorWhoCoreDbContext dbContext)
    {
        Console.WriteLine("\nCalling spSummariseEpisodes:");

        using (var command = dbContext.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "EXEC spSummariseEpisodes";

            dbContext.Database.OpenConnection();

            using (var reader = command.ExecuteReader())
            {
                // Retrieve the result for companions
                var companionsResult = new List<CompanionSummary>();
                while (reader.Read())
                {
                    var companionName = reader.GetString(0);
                    var numAppearances = reader.GetInt32(1);

                    var companionSummary = new CompanionSummary
                    {
                        CompanionName = companionName,
                        NumAppearances = numAppearances
                    };

                    companionsResult.Add(companionSummary);
                }

                // Display the companions result
                Console.WriteLine("Top 3 Companions:");
                foreach (var companion in companionsResult)
                {
                    Console.WriteLine($"Companion Name: {companion.CompanionName}, Appearances: {companion.NumAppearances}");
                }

                if (reader.NextResult())
                {
                    // Retrieve the result for enemies
                    var enemiesResult = new List<EnemySummary>();
                    while (reader.Read())
                    {
                        var enemyName = reader.GetString(0);
                        var numAppearances = reader.GetInt32(1);

                        var enemySummary = new EnemySummary
                        {
                            EnemyName = enemyName,
                            NumAppearances = numAppearances
                        };

                        enemiesResult.Add(enemySummary);
                    }

                    // Display the enemies result
                    Console.WriteLine("Top 3 Enemies:");
                    foreach (var enemy in enemiesResult)
                    {
                        Console.WriteLine($"Enemy Name: {enemy.EnemyName}, Appearances: {enemy.NumAppearances}");
                    }
                }
            }
        }

        Console.WriteLine("spSummariseEpisodes executed successfully.");
    }










}