using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SideProjectHelper.Data;
using SideProjectHelper.Models;

namespace SideProjectHelper.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Project
        public async Task<IActionResult> Index()
        {
            return View(await _context.Project.ToListAsync());
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,Title,Description")] Project project, IFormFile? Photo)
        {
            // TODO: change User according to current login user
            // var user = _context.User.FirstOrDefault();
            // project.User = user;
            // ModelState["User"].ValidationState = ModelValidationState.Valid;
            
            
            if (ModelState.IsValid)
            {
                
                // check for photo & upload if exists, then capture new unique file name
                if (Photo != null)
                {
                    var fileName = UploadPhoto(Photo);
                    project.Photo = fileName;
                }
                
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Title,Description")] Project project, IFormFile? Photo, string CurrentPhoto)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }
            // TODO: change User according to current login user
            // var user = _context.User.FirstOrDefault();
            // project.User = user;
            // ModelState["User"].ValidationState = ModelValidationState.Valid;
            if (ModelState.IsValid)
            {
                try
                {
                    // check for photo & upload if exists, then capture new unique file name
                    if (Photo != null)
                    {
                        var fileName = UploadPhoto(Photo);
                        project.Photo = fileName;
                    }
                    else
                    {
                        // if there is a photo, project doesn't contain the filename string
                        project.Photo = CurrentPhoto;
                    }

                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
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
            return View(project);
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            if (project != null)
            {
                _context.Project.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }
        
        // reference
        // Richard Freeman - https://github.com/ifotn/ThreadHaven
        // https://sd.blackball.lv/en/articles/read/19650-file-upload-in-aspnet-core-6-detailed-guide
        private static string UploadPhoto(IFormFile photo)
        {
            // get temp location of uploaded photo
            // var filePath = Path.GetTempFileName();

            // use GUID (globally unique identifier) to create unique file name
            // eg. product.jpg => abc123-product.jpg
            var fileName = Guid.NewGuid() + "-" + photo.FileName;

            // set destination path dynamically
            // var uploadPath = System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\img\\project\\" + fileName;
            var path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", "img", "project");
            // we should ensure the Directory is exist
            if (!Directory.Exists(Path.GetFullPath(path)))
            {
                // if not, create Directory
                Directory.CreateDirectory(path);
            }

            var uploadPath = Path.Combine(path, fileName);
            // copy the file 
            // using: the using statement ensures that a disposable instance is disposed
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/using
            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                photo.CopyTo(stream);
            }

            // send back new unique file name
            return fileName;
        }
    }
}
