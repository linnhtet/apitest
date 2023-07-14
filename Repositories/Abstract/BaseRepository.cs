using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Repositories.Abstract
{
    public class BaseRepository<T> where T : class
    {
        protected DataContext _context;

        protected BaseRepository(DataContext context)
        {
            _context = context;
        }

        public void AddToDB(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
            }
            catch (DbUpdateException ex)
            {
                throw new AppException(ex.InnerException.Message);
            }
            catch (AppException ex)
            {
                throw new AppException(ex.Message);
            }
        }
        public void AddRangeToDB(IEnumerable<T> entity)
        {
            try
            {
                _context.Set<T>().AddRange(entity);
            }
            catch (DbUpdateException ex)
            {
                throw new AppException(ex.InnerException.Message);
            }
            catch (AppException ex)
            {
                throw new AppException(ex.Message);
            }
        }

        public void CommitToDB()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new AppException(ex.InnerException.Message);
            }
            catch (AppException ex)
            {
                throw new AppException(ex.Message);
            }
        }
    }
}
