using Microsoft.EntityFrameworkCore;
using Portal.Database;
using Portal.Intefaces;
using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Repositories
{
    public class NewsRepository:INewsRepository
    {
        private AppDbContext _context;
            public NewsRepository(AppDbContext context) {

            _context = context;
           }

        public async Task<List<News>> GetAllNews()
        {
            try
            {
                List<News> news = _context.news.Include(x=> x.Category).Where(x => x.IsDeleted == false && x.Category.ShowOnline==true).ToList();
                return  news;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<News> GetNewsById(int Id)
        {
            try
            {
                var news=_context.news.Include(x => x.Category).Where(x => x.IsDeleted == false && x.Category.ShowOnline == true && x.NewsId==Id).FirstOrDefault();
                return news;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task AddorEditNews(News news)
        {
            try
            {
                if (news.NewsId == 0)
                {
                    news.CreatedOnDate = DateTime.Now;
                   


                    _context.Add(news);
                }
                else
                {
                    news.UpdatedOnDate = DateTime.Now;
                   
                    _context.Update(news);
                }
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task  DeleteNews(int id)
        {
            try
            {
                var news = _context.news.Where(x => x.NewsId == id).FirstOrDefault();
                if (news != null)
                {
                    news.IsDeleted = true;
                    _context.Update(news);
                    _context.SaveChanges();
                }


            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Watched>> getWatchedNews(int id)
        {
            try
            {
                var wn = _context.watcheds.Where(x => x.newsId == id).ToList();

                return wn;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Views>> getViews()
        {
            try
            {
                var model = _context.watcheds.Include(x => x.News).Include(x => x.User).ToList().GroupBy(x => x.newsId);
                var views = new List<Views>();

                foreach (var item in model)
                {
                    views.Add(new Views() { id = item.Key, NewsTitle = item.FirstOrDefault().News.Title, nrOfClicks = item.Count() });
                }




                return views;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<News>> getSavedNews(string UserId)
        {
            var saved = _context.saved.Where(x=> x.UserId==UserId).Include(x => x.News).Select(x => x.News).ToList();

            return saved;
           
        }
        
        public async Task addView(int NewsId, string UserId,string FingerPrintId)
        {
            try
            {
                //var news=_context.news.Where(x=>x.NewsId==NewsId).FirstOrDefault();
                var watched = new Watched() { newsId = NewsId, userId = UserId, FingerPrintId = FingerPrintId, watchedOn = DateTime.Now };

                _context.watcheds.Add(watched);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task addSaved(int NewsId, string UserId)
        {
            try
            {
                var saved = new SavedNews()
                {
                    NewsId = NewsId,
                    UserId = UserId
                };
                _context.saved.Add(saved);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task delete(int newsId,string UserId)
        {
            try
            {
                var news = _context.saved.Where(x => x.NewsId == newsId && x.UserId == UserId).FirstOrDefault();
                if (news != null)
                {
                    _context.Remove(news);
                    _context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task addReaction(Reaction r)
        {
            try
            {
                _context.reaction.Add(r);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;

            }
        }
        public async  Task<List<Reaction>> getReactionsByNeews(int id)
        {
            try
            {
                var reactions = _context.reaction.Where(x => x.newsId == id).Include(x=> x.user).ToList();
                return reactions;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ReactionVIewModel>> GetAllReactions()
        {
            try
            {
                var reactions = _context.reaction.ToList().GroupBy(x=>x.newsId);
                List<ReactionVIewModel> model = new List<ReactionVIewModel>();
                foreach(var item in reactions)
                {
                    model.Add(new ReactionVIewModel { NewsId = item.FirstOrDefault().newsId, Sad = item.Where(x => x.reaction == 2).Count(), Happy = item.Where(x => x.reaction == 1).Count(), Angry = item.Where(x => x.reaction == 3).Count() });

                }
                return model;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task updateFingerPrint(string fingerprintId, string userId)
        {
            try
            {
                var views = _context.watcheds.Where(x => x.FingerPrintId == fingerprintId && x.userId==null).ToList();

                views.ForEach(x => x.userId = userId);

                _context.watcheds.UpdateRange(views);
                _context.SaveChanges();

            }
            catch(Exception ex)
            {
                throw ex;
            }


        }

        public async Task<List<News>> GetByTag(string tag)
        {
            try
            {
                var tags = new List<string>();
                var news = _context.news.Where(x=> x.tags.Contains(tag)).ToList();

                return news;

               

            }
            catch(Exception ex)
            {
                throw ex;          
            }
        }
       public async  Task<List<News>> GetMostWatched()
        {
            try
            {
                var news = _context.watcheds.Include(x => x.News).Select(x => x.News).ToList();
                return news;
            }
            catch(Exception ex)
            {
                throw ex;            }
        }





    }
}
