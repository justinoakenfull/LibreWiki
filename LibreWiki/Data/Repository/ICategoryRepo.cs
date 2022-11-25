using System.Collections.Generic;
using System.Threading.Tasks;
using LibreWiki.Models;

namespace LibreWiki.Data.Repository
{
    public interface ICategoryRepo
    {
        Task<OperationResult<List<Category>>> GetAllCategoriesAndSubCategories();
        Task<OperationResult<List<Category>>> GetAllCategories();
        Task<OperationResult<Category>> GetCategory(int id);
        Task<OperationResult<Category>> GetCategoryByName(string name);
        Task<OperationResult<Category>> CreateCategory(Category category);
        Task<OperationResult<Category>> UpdateCategory(Category category);
        Task<OperationResult<Category>> DeleteCategoryById(int id);


    }
}