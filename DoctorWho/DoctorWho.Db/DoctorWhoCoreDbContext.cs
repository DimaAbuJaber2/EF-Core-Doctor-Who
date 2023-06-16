using Microsoft.EntityFrameworkCore;

namespace DoctorWho.Db
{
    public class DoctorWhoCoreDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\ProjectModels;Initial Catalog=DoctorWho;Integrated Security=True");
        }


    }
}