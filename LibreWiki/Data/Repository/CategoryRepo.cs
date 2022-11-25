using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using LibreWiki.Models;
using Microsoft.EntityFrameworkCore;

namespace LibreWiki.Data.Repository
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly WikiContext _categoryContext;
        public CategoryRepo(WikiContext categoryContext)
        {
            _categoryContext = categoryContext;
        }

        /**
         * Gets all parent categories and their sub categories included. Returns the OperationResult with a success or error.
         */
        public async Task<OperationResult<List<Category>>> GetAllCategoriesAndSubCategories()
        {
            try
            {
                var categoryList = await _categoryContext.Categories.Where(c => c.ParentCategoryId == null || c.ParentCategoryId == 0).Include(c => c.ChildenCategories).ToListAsync();

                if (categoryList == null)
                {
                    return new OperationResult<List<Category>>()
                    {
                        Success = false,
                        ErrorMessage = "No categories were found."
                    };
                }

                return new OperationResult<List<Category>>()
                {
                    Success = true,
                    ObjectResult = categoryList
                };
            }
            catch (Exception e)
            {
                return TryCatchFailedListCategory(e);
            }
        }

        /**
         * Gets all categories and returns them in the operation result wrapper.
         */
        public async Task<OperationResult<List<Category>>> GetAllCategories()
        {
            try
            {
                var allCategories = await _categoryContext.Categories.ToListAsync();

                if (allCategories == null)
                {
                    return new OperationResult<List<Category>>()
                    {
                        Success = false,
                        ErrorMessage = "No categories were found."
                    };
                }

                return new OperationResult<List<Category>>
                {
                    Success = true,
                    ObjectResult = allCategories
                };
            }
            catch (Exception e)
            {
                return TryCatchFailedListCategory(e);
            }
        }

        /// <summary>
        /// Gets a category by its ID from the repository.
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Operation Result with success or error message including the resulting object.</returns>
        public async Task<OperationResult<Category>> GetCategory(int id)
        {
            try
            {
                var categoryById = await _categoryContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

                if (categoryById == null)
                {
                    return new OperationResult<Category>()
                    {
                        Success = false,
                        ErrorMessage = "Category not Found."
                    };
                }

                return new OperationResult<Category>()
                {
                    Success = true,
                    ObjectResult = categoryById
                };
            }
            catch (Exception e)
            {
                return TryCatchFailedCategory(e);
            }
        }

        public async Task<OperationResult<Category>> GetCategoryByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new OperationResult<Category>()
                {
                    Success = false,
                    ErrorMessage = "Search string was empty or not valid."
                };
            }

            try
            {
                var category = await _categoryContext.Categories.FirstOrDefaultAsync(c =>c.Name.Equals(name));

                if (category == null)
                {
                    return new OperationResult<Category>()
                    {
                        Success = false,
                        ErrorMessage = "No category was found with the name."
                    };
                }

                return new OperationResult<Category>()
                {
                    Success = true,
                    ObjectResult = category
                };
            }
            catch (Exception e)
            {
                return TryCatchFailedCategory(e);
            }
        }

        public async Task<OperationResult<Category>> CreateCategory(Category category)
        {
            if (category == null)
            {
                return new OperationResult<Category>
                    { 
                        Success = false, 
                        ErrorMessage = "Given category was empty or not valid."
                    };
            }

            try
            {
                _categoryContext.Categories.Add(category);
                await _categoryContext.SaveChangesAsync();
                return new OperationResult<Category>
                {
                    Success = true, 
                    ObjectResult = category
                };

            }
            catch (Exception e)
            {
                return TryCatchFailedCategory(e);
            }
        }

        public async Task<OperationResult<Category>> UpdateCategory(Category category)
        {
            if (category == null)
            {
                return new OperationResult<Category>
                {
                    Success = false,
                    ErrorMessage = "Given category was empty or not valid."
                };
            }

            try
            {
                var foundCategory = await _categoryContext.Categories.Where(c => c.Id == category.Id).FirstOrDefaultAsync();

                if (foundCategory == null)
                {
                    return new OperationResult<Category>
                    {
                        Success = false,
                        ErrorMessage = "No category found to update."
                    };
                }

                _categoryContext.Categories.Update(foundCategory);
                await _categoryContext.SaveChangesAsync();

                return new OperationResult<Category>
                {
                    Success = true,
                    ObjectResult = foundCategory
                };
            }
            catch (Exception e)
            {
                return TryCatchFailedCategory(e);
            }
        }

        public async Task<OperationResult<Category>> DeleteCategoryById(int id)
        {
            if (id == 0 || id == null)
            {
                return new OperationResult<Category>
                {
                    Success = false,
                    ErrorMessage = "Given ID was incorrect or not valid."
                };
            }

            try
            {
                var foundCategory = _categoryContext.Categories.FirstOrDefault(c => c.Id == id);
                if (foundCategory == null)
                {
                    return new OperationResult<Category>
                    {
                        Success = false,
                        ErrorMessage = "No category found with the ID to delete."
                    };
                }

                _categoryContext.Categories.Remove(foundCategory);
                await _categoryContext.SaveChangesAsync();

                return new OperationResult<Category>
                {
                    Success = true,
                    ObjectResult = foundCategory
                };
            }
            catch (Exception e)
            {
                return TryCatchFailedCategory(e);
            }
        }

        private static OperationResult<Category> TryCatchFailedCategory(Exception exception)
        {
            return new OperationResult<Category>
            {
                Success = false,
                ErrorMessage = exception.Message
            };
        }

        private static OperationResult<List<Category>> TryCatchFailedListCategory(Exception exception)
        {
            return new OperationResult<List<Category>>
            {
                Success = false,
                ErrorMessage = exception.Message
            };
        }
    }
}
