using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorWho.Db.Repository
{
    public class AuthorRepository
    {
        private readonly DoctorWhoCoreDbContext _dbContext;

        public AuthorRepository(DoctorWhoCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int CreateAuthor(Author author)
        {
            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();
            return author.AuthorId;
        }

        public void UpdateAuthor(Author author)
        {
            _dbContext.Authors.Update(author);
            _dbContext.SaveChanges();
        }

        public void DeleteAuthor(int id)
        {
            var author = _dbContext.Authors.Find(id);
            if (author != null)
            {
                _dbContext.Authors.Remove(author);
                _dbContext.SaveChanges();
            }
        }

        public Author GetAuthorById(int authorId)
        {
            return _dbContext.Authors.FirstOrDefault(c => c.AuthorId == authorId);
        }
    }
}
