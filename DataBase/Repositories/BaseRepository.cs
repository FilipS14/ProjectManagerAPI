using DataBase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Repositories
{
    public class BaseRepository
    {
        protected ProjectDbContext _db { get; set; }
        public BaseRepository(ProjectDbContext db) { _db = db; }
        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
