using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Intefaces
{
    public interface INewsRepository
    {
        Task<List<News>> GetAllNews();
        Task<News> GetNewsById(int Id);
        Task AddorEditNews(News news);
        Task DeleteNews(int id);
        Task<List<Watched>> getWatchedNews(int id);
        Task<List<News>> getSavedNews(string UserId);
        Task addView(int NewsId,string UserId,string FingerPrintId);
        Task addSaved(int NewsId,string UserId);
        Task<List<Views>> getViews();
        Task delete(int newsId, string UserId);
        Task<List<ReactionVIewModel>> GetAllReactions();
        Task<List<Reaction>> getReactionsByNeews(int id);
        Task addReaction(Reaction r);
        Task updateFingerPrint(string fingerprintId, string userId);
        Task<List<News>> GetByTag(string tag);
        Task<List<News>> GetMostWatched();


    }
}
