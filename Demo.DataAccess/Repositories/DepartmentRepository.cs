using Demo.DataAccess.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataAccess.Repositories
{
    public class DepartmentRepository(ApplicationDbContext context) : IDepartmentRepository
    {
        //ApplicationDbContext context = new ApplicationDbContext();


        private readonly ApplicationDbContext _context = context;

        //GET
        public Department? GetById(int id) => _context.Departments.Find(id);
        //GET ALL
        public IEnumerable<Department> GetAll(bool withTracking = false)
            => withTracking ? _context.Departments.Where(D => !D.IsDeleted).ToList() :
            _context.Departments.AsNoTracking().Where(D => !D.IsDeleted).ToList();
        //ADD
        public int Add(Department department)
        {
            _context.Departments.Add(department);
            return _context.SaveChanges();
        }
        //UPDATE

        public int Update(Department department)
        {
            _context.Departments.Update(department);
            return _context.SaveChanges();
        }
        //DELETE

        public int Delete(Department department)
        {
            _context.Departments.Remove(department);
            return _context.SaveChanges();
        }
    }
}
