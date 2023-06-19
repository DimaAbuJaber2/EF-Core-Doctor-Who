using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorWho.Db.Repository
{
    public class CompanionRepository
    {
        private readonly DoctorWhoCoreDbContext _dbContext;

        public CompanionRepository(DoctorWhoCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int CreateCompanion(Companion companion)
        {
            _dbContext.Companions.Add(companion);
            _dbContext.SaveChanges();
            return companion.CompanionId;
        }

        public void UpdateCompanion(Companion companion)
        {
            _dbContext.Companions.Update(companion);
            _dbContext.SaveChanges();
        }

        public void DeleteCompanion(int id)
        {
            var companion = _dbContext.Companions.Find(id);
            if (companion != null)
            {
                _dbContext.Companions.Remove(companion);
                _dbContext.SaveChanges();
            }
        }
        public Companion GetCompanionById(int companionId)
        {
            return _dbContext.Companions.FirstOrDefault(c => c.CompanionId == companionId);
        }
    }
}
