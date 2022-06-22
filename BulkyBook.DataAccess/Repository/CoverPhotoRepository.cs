using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;


namespace BulkyBook.DataAccess.Repository
{
    public class CoverPhotoRepository : Repository<CoverPhoto>, ICoverPhotoRepository
    {
        private ApplicationDBContext _db;

        public CoverPhotoRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }


        public void Update(CoverPhoto obj)
        {
            _db.CoverPhoto.Update(obj);
        }
    }
}