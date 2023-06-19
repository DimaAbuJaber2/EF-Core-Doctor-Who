using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorWho.Db.Repository
{
    public class DoctorRepository
    {
        private readonly DoctorWhoCoreDbContext _dbContext;

        public DoctorRepository(DoctorWhoCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int CreateDoctor(Doctor doctor)
        {
            _dbContext.Doctors.Add(doctor);
            _dbContext.SaveChanges();
            return doctor.DoctorId;
        }

        public void UpdateDoctor(Doctor doctor)
        {
            _dbContext.Doctors.Update(doctor);
            _dbContext.SaveChanges();
        }

        public void DeleteDoctor(int id)
        {
            var doctor = _dbContext.Doctors.Find(id);
            if (doctor != null)
            {
                _dbContext.Doctors.Remove(doctor);
                _dbContext.SaveChanges();
            }
        }


        public Doctor GetDoctorById(int doctorId)
        {
            return _dbContext.Doctors.FirstOrDefault(c => c.DoctorId == doctorId);
        }
    }
}
