using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorWho.Db.Repository
{
   public class EnemyRepository
    {
        private readonly DoctorWhoCoreDbContext _dbContext;

        public EnemyRepository(DoctorWhoCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int CreateEnemy(Enemy enemy)
        {
            _dbContext.Enemies.Add(enemy);
            _dbContext.SaveChanges();
            return enemy.EnemyId;
        }

        public void UpdateEnemy(Enemy enemy)
        {
            _dbContext.Enemies.Update(enemy);
            _dbContext.SaveChanges();
        }

        public void DeleteEnemy(int id)
        {
            var enemy = _dbContext.Enemies.Find(id);
            if (enemy != null)
            {
                _dbContext.Enemies.Remove(enemy);
                _dbContext.SaveChanges();
            }
        }

        public Enemy GetEnemyById(int enemyId)
        {
            return _dbContext.Enemies.FirstOrDefault(c => c.EnemyId == enemyId);
        }

    }
}
