using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorWho.Db.Repository
{
    public class EpisodeRepository
    {
        private readonly DoctorWhoCoreDbContext _dbContext;

        public EpisodeRepository(DoctorWhoCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int CreateEpisode(Episode episode)
        {
            _dbContext.Episodes.Add(episode);
            _dbContext.SaveChanges();
            return episode.EpisodeId;
        }

        public void UpdateEpisode(Episode episode)
        {
            _dbContext.Episodes.Update(episode);
            _dbContext.SaveChanges();
        }

        public void DeleteEpisode(int id)
        {
            var episode = _dbContext.Episodes.Find(id);
            if (episode != null)
            {
                _dbContext.Episodes.Remove(episode);
                _dbContext.SaveChanges();
            }
        }
        public Episode GetEpisodeById(int episodeId)
        {
            return _dbContext.Episodes.FirstOrDefault(c => c.EpisodeId == episodeId);
        }

        public void AddEnemyToEpisode(int episodeId, EpisodeEnemy enemy)
        {
            var episode = _dbContext.Episodes.Include(e => e.EpisodeEnemies).FirstOrDefault(e => e.EpisodeId == episodeId);
            if (episode != null)
            {
                episode.EpisodeEnemies.Add(enemy);
                _dbContext.SaveChanges();
            }
        }

        public void AddCompanionToEpisode(int episodeId, EpisodeCompanion companion)
        {
            var episode = _dbContext.Episodes.Include(e => e.EpisodeCompanions).FirstOrDefault(e => e.EpisodeId == episodeId);
            if (episode != null)
            {
                episode.EpisodeCompanions.Add(companion);
                _dbContext.SaveChanges();
            }
        }
    }
}
