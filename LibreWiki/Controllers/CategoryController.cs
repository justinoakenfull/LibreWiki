using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibreWiki.Data;
using LibreWiki.Data.Repository;
using LibreWiki.Models;

namespace LibreWiki.Controllers
{
    public class CategoryController : Controller
    {
        //private readonly WikiContext _context;
        private readonly ICategoryRepo _categoryRepo;

        public CategoryController(WikiContext context, ICategoryRepo categoryRepo)
        {
            //_context = context;
            _categoryRepo = categoryRepo;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var wikiContext = await _categoryRepo.GetAllCategoriesAndSubCategories();

            if (wikiContext.Success)
            {
                return View(wikiContext.ObjectResult);
            }

            return View();
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryResult = await _categoryRepo.GetCategory((int)id);

            if (!categoryResult.Success)
            {
                return BadRequest(categoryResult.ErrorMessage);
            }

            return View(categoryResult.ObjectResult);
        }

        // GET: Category/Create
        public async Task<IActionResult> CreateAsync()
        {
            var allCategoriesResult = await _categoryRepo.GetAllCategories();

            if (allCategoriesResult.Success)
            {
                ViewData["ParentCategoryId"] = new SelectList(allCategoriesResult.ObjectResult, "Id", "Name");
            }
            
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,ParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryRepo.CreateCategory(category);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                return BadRequest(result.ErrorMessage);
            }

            var allCategoriesResult = await _categoryRepo.GetAllCategories();

            if (allCategoriesResult.Success)
            {
                ViewData["ParentCategoryId"] = new SelectList(allCategoriesResult.ObjectResult, "Id", "Name", category.ParentCategoryId);
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Name", category.ParentCategoryId);
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ParentCategoryId")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Name", category.ParentCategoryId);
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        public async Task<IActionResult> _CategoryList()
        {
            var wikiContext = await _context.Categories.Where(c => c.ParentCategoryId == null || c.ParentCategoryId == 0).Include(c => c.ChildenCategories).ToListAsync();
            return PartialView(wikiContext);
        }
    }
}
