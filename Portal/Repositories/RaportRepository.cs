using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portal.Database;
using Portal.Intefaces;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Repositories
{
    public class RaportRepository:IRaportRepository
    {
        private AppDbContext dbContext;
        private UserManager<IdentityUser> userManager;
        public RaportRepository(AppDbContext context,UserManager<IdentityUser> users) {
            dbContext = context;
            userManager = users;
        }

        public async Task<DashboardViewModel> GetDashboardVIewModel()
        {
            var model = new DashboardViewModel();
            try
            {


                model.Categories =  dbContext.categories.Count();
                model.News =  dbContext.news.Count();
                model.Users = userManager.Users.Count();
                model.Saved = dbContext.saved.Count();
                model.VIews = dbContext.watcheds.Count();
                model.Angry = dbContext.reaction.Where(x => x.reaction == 3).Count();
                model.Sad = dbContext.reaction.Where(x => x.reaction == 2).Count();
                model.Happy = dbContext.reaction.Where(x => x.reaction == 2).Count();
                var list = await userManager.GetUsersInRoleAsync("Admin");
                model.Admins = list.Count();

                return model;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
       public async  Task<FirstRaportViewModel> GetFirstRaportViewModel(FirstRaportViewModel model)
        {
            try
            {
                var result = new List<FirstRaportResultViewModel>();
                if (model.mainElement == "users")
                {
                    if (model.whatToShow == "views")
                    {
                        var res = dbContext.watcheds.Where(x => x.watchedOn >= model.from && x.watchedOn <= model.to && x.userId != null).Include(x => x.User).ToList().GroupBy(x => x.userId);
                        foreach (var item in res)
                        {
                            result.Add(new FirstRaportResultViewModel { Id = item.FirstOrDefault().userId, name = item.FirstOrDefault().User.UserName, number = item.Count() });
                        }
                    }
                    else if (model.whatToShow == "saved")
                    {
                        var res = dbContext.saved.Include(x => x.User).ToList().GroupBy(x => x.UserId);
                        foreach (var item in res)
                        {
                            result.Add(new FirstRaportResultViewModel { Id = item.FirstOrDefault().UserId, name = item.FirstOrDefault().User.UserName, number = item.Count() });
                        }
                    }
                    else if (model.whatToShow == "reactions")
                    {
                        var res = dbContext.reaction.Include(x => x.user).ToList().GroupBy(x => x.userId);
                        foreach (var item in res)
                        {
                            result.Add(new FirstRaportResultViewModel { Id = item.FirstOrDefault().userId, name = item.FirstOrDefault().user.UserName, number = item.Count() });
                        }

                    }
                }
                else if (model.mainElement == "news")
                {
                    if (model.whatToShow == "views")
                    {
                        var res = dbContext.watcheds.Where(x => x.watchedOn >= model.from && x.watchedOn <= model.to).Include(x => x.News).ToList().GroupBy(x => x.newsId);
                        foreach (var item in res)
                        {
                            result.Add(new FirstRaportResultViewModel { Id = item.FirstOrDefault().newsId.ToString(), name = item.FirstOrDefault().News.Title, number = item.Count() });
                        }
                    }
                    else if (model.whatToShow == "saved")
                    {
                        var res = dbContext.saved.Include(x => x.News).ToList().GroupBy(x => x.NewsId);
                        foreach (var item in res)
                        {
                            result.Add(new FirstRaportResultViewModel { Id = item.FirstOrDefault().NewsId.ToString(), name = item.FirstOrDefault().News.Title, number = item.Count() });
                        }
                    }
                    else if (model.whatToShow == "reactions")
                    {
                        var res = dbContext.reaction.Include(x => x.news).ToList().GroupBy(x => x.newsId);
                        foreach (var item in res)
                        {
                            result.Add(new FirstRaportResultViewModel { Id = item.FirstOrDefault().newsId.ToString(), name = item.FirstOrDefault().news.Title, number = item.Count() });
                        }

                    }
                }
                model.result = result;
                return model;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}