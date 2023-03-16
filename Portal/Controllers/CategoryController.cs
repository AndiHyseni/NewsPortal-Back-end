using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portal.Database;
using Portal.Intefaces;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
      private readonly  ICategories _categories;
        public CategoryController(ICategories categories)
        {
            _categories = categories;
        }
        
        
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {

            return  await _categories.getAllCategories();

        }
        [HttpGet("/getCategoriesOnline")]
        public async Task<ActionResult<List<Category>>> GetCategoriesOnline()
        {

            var categ= await _categories.getAllCategories();
            categ = categ.Where(x => x.ShowOnline == true).ToList();
            return categ;

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Add(Category category)
        {
            if (ModelState.IsValid)
            {
              var added= _categories.addCategory(category);
               if( added.IsCompletedSuccessfully)
                {
                    return Ok();
                }
                return BadRequest("Smething went wrong");
            }
            return BadRequest("Something went wrong");
            
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<Category>> GetCategory(int Id)
        {

            var categ= await _categories.GetCategoryById(Id);

            if (categ == null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(categ);

        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
           var deleted= _categories.DeleteCategory(id);

            if (deleted.IsCompletedSuccessfully)
            {
                return Ok();
            }
            return BadRequest("Something went Wrong");
        }
    }
}
