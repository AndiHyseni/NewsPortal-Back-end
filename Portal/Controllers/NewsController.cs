using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portal.Intefaces;
using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private INewsRepository _newsRepository;
        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        
        [HttpGet]
        
        public async Task<ActionResult<List<News>>> GetAllNews()
        {
            var news=await _newsRepository.GetAllNews();
            if (news == null)
            {
                return BadRequest("Something Went Wrong");
            }

            return Ok(news);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNewsById(int id)
        {
           var news=await _newsRepository.GetNewsById(id);


            if (news == null)
            {
                return BadRequest("Something Went Wrong");
            }

            return Ok(news);
        }
        [HttpPost]
        [Route("/addnews")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddOrEdit(News news)
        {
            var task=_newsRepository.AddorEditNews(news);

            if (task.IsCompletedSuccessfully)
            {
                return Ok();
            }
            return BadRequest("Something went Wrong");
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteNews(int id)
        {
          var task=  _newsRepository.DeleteNews(id);
            if(task.IsCompletedSuccessfully)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("/addView")]
       
        public async Task<ActionResult> AddWatched(Watched watched)
        {
           
            var task= _newsRepository.addView(watched.newsId, watched.userId, watched.FingerPrintId);
            if (task.IsCompletedSuccessfully)
            {
                return Ok();
            }
            return BadRequest();


        }
        [HttpPost]
        [Route("/addSavedNews")]

        public async Task<ActionResult> AddSavedNews(SavedNews sn)
        {
           
              var task=  _newsRepository.addSaved(sn.NewsId, sn.UserId);
            if (task.IsCompletedSuccessfully)
            {
                return Ok();
            }
            return BadRequest();


        }
        [HttpGet]
        [Route("/getwatched/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Watched>>> getWatched(int id)
        {
           var items=await  _newsRepository.getWatchedNews(id);
            if (items == null)
            {
                return BadRequest("Something Went Wrong");
            }
            return Ok(items);

        }
        [HttpGet]
        [Route("/getViews")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Views>>> getViews()
        {
           var views=await  _newsRepository.getViews();

            if (views == null)
            {
                return BadRequest();
            }
            return Ok(views);
        }
        [HttpGet]
        [Route("/getSaved/{userId}")]
        [Authorize]
        public async Task<ActionResult<List<News>>> GetSavedNews(string UserId)
        {
           var news= await  _newsRepository.getSavedNews(UserId);
            if (news == null)
            {
                return BadRequest("Something Went Wrong");
            }
            return Ok(news);
        }
        [HttpPost]

        [Route("/deleteSaved")]
        [Authorize]
        public async Task<ActionResult> Delete(SavedNews sn)
        {
            var task=_newsRepository.delete(sn.NewsId, sn.UserId);
            if (task.IsCompletedSuccessfully)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("/addReaction")]
        [Authorize]
        public async Task<ActionResult> AddReaction(Reaction r)
        {
          var tsk=  _newsRepository.addReaction(r);
            if (tsk.IsCompletedSuccessfully)
            {
                return Ok();
            }
            return BadRequest("SomethingWentWrong");
        }
        [HttpGet]
        [Route("/getreactions/{id}")]
        public async Task<ActionResult<List<Reaction>>> getReactionForNews(int id)
        {
            var reactions=await _newsRepository.getReactionsByNeews(id);
            if(reactions==null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(reactions);
        }
        [HttpGet]
        [Route("/reaction")]
        public async Task<ActionResult<List<ReactionVIewModel>>> getReactions()
        {
            var response= await _newsRepository.GetAllReactions();

            if(response==null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(response);
        }
       [HttpGet]
       [Route("/tags/{tag}")]
       public async Task<ActionResult<List<News>>> GetByTag(string tag)
        {
            var news =await _newsRepository.GetByTag(tag);
            if (news == null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(news);

        }
        [HttpGet]
        [Route("/mostviewed")]
        public async Task<ActionResult<List<News>>> GetMostViewed()
        {
            var news = await _newsRepository.GetMostWatched();
            if (news == null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(news);

        }

    }

    }

