using Shoper.BusinessLogic.Interface;
using Shoper.Data.Interface;
using Shoper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.BusinessLogic.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        // data da var olan repositoryleri kapsülleme yaparak burada kullandık
        // servis de var olan bilgiler alınarak servis edilir 
        public CategoryService(ICategoryRepository categoryRepository)
        {
            // ctor da yazdık çünkü açılır açılmaz burası çalışsın ve repositorylerin örneği alınsın 

            this._categoryRepository=categoryRepository;
        }
        public Category Add(Category entity)
        {
           return _categoryRepository.Add(entity);
        }

        public Category Delete(Category entity )
        {
            return _categoryRepository.Delete(entity);
        }

        public Category Get(int id)
        {
            return _categoryRepository.GetbyId(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public IEnumerable<Category> GetExp(Expression<Func<Category, bool>> predicate)
        {
           return _categoryRepository.GetExp(predicate);
        }

        public Category Update(Category entity)
        {
            return _categoryRepository.Update(entity);
        }
    }
}
