// See https://aka.ms/new-console-template for more information
using DoctorWho.Db;
using DoctorWho.Db.DTO;
using DoctorWho.Db.Repository;
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


        var companionRepository = new CompanionRepository(dbContext);
        var enemyRepository = new EnemyRepository(dbContext);
        var doctorRepository = new DoctorRepository(dbContext);
        var authorRepository = new AuthorRepository(dbContext);
        var episodeRepository = new EpisodeRepository(dbContext);

        Console.WriteLine("-------------------------------------");

        var newCompanion = new Companion
        {
            CompanionName = "New Companion Name2",
            WhoPlayed = "Actor Name2"
        };
        int companionId = companionRepository.CreateCompanion(newCompanion);
        Console.WriteLine($"New companion created with ID: {companionId}");

        var existingCompanion = companionRepository.GetCompanionById(companionId);
        if (existingCompanion != null)
        {
            // Update the companion properties
            existingCompanion.CompanionName = "Updated Companion Name";
            existingCompanion.WhoPlayed = "Updated Actor Name";
            companionRepository.UpdateCompanion(existingCompanion);
            Console.WriteLine("Companion updated successfully.");
        }

        // Delete a companion
        companionRepository.DeleteCompanion(companionId);
        Console.WriteLine("Companion deleted successfully.");

        Console.WriteLine("-------------------------------------");


        // Create an author
        var newAuthor = new Author
        {
            AuthorName = "New Author Name"
        };
      int authorId= authorRepository.CreateAuthor(newAuthor);

        // Update an author
        var existingAuthor = authorRepository.GetAuthorById(authorId);
        if (existingAuthor != null)
        {
            existingAuthor.AuthorName = "Updated Author Name";
            authorRepository.UpdateAuthor(existingAuthor);
        }

        // Delete an author
        authorRepository.DeleteAuthor(authorId);


        Console.WriteLine("-------------------------------------");

        int episodeId = 1;
        int enemyId = 1; 
        var episodeEnemy = new EpisodeEnemy
        {
            EpisodeId = episodeId,
            EnemyId = enemyId
        };
        episodeRepository.AddEnemyToEpisode(episodeId, episodeEnemy);

        Console.WriteLine("-------------------------------------");
        int comId = 1; 
        var episodeCompanion = new EpisodeCompanion
        {
            EpisodeId = episodeId,
            CompanionId = comId
        };
        episodeRepository.AddCompanionToEpisode(episodeId, episodeCompanion);

        Console.WriteLine("-------------------------------------");
        var doctors = doctorRepository.GetAllDoctors();
        Console.WriteLine("List of Doctors:");
        foreach (var doctor in doctors)
        {
            Console.WriteLine($"Doctor ID: {doctor.DoctorId}");
            Console.WriteLine($"Doctor Name: {doctor.DoctorName}");
            Console.WriteLine();
        }





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