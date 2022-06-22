using System;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface ICoverPhotoRepository : IRepository<CoverPhoto>
    {
        void Update(CoverPhoto obj);
    }
}
