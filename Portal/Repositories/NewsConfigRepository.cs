using Portal.Database;
using Portal.Intefaces;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Repositories
{
    public class NewsConfigRepository:INewsConfig
    {
        private AppDbContext _context;
        public NewsConfigRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<NewsConfig> GetConfigRow() 
        {
            try
            {
                NewsConfig news = _context.newsConfigs.FirstOrDefault(); 
                return news;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task EditConfigRow(NewsConfig newsCfg)
        {
            try
            {
               
                _context.Update(newsCfg);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        
    }
}
