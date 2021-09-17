using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary.DataContext;
using VideoLibrary.Models;

namespace VideoLibrary.Repository
{
    public class VideoRepo : IVideoRepo
    {
        private readonly DapperContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<VideoRepo> _log;
        public VideoRepo(IConfiguration configuration, DapperContext context,
            ILogger<VideoRepo> log)
        {
            _config = configuration;
            _context = context;
            _log = log;
        }

        public async Task<Video> SaveVideoAsync(Video videoData) {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var id = await connection.InsertAsync<Video>(videoData);
                    var video = await connection.GetAsync<Video>(id);
                    return video;
                }

            }
            catch (Exception ex)
            {
                _log.LogError("Error saving", ex);
            }
            return null;
        }

        public async Task<Video> LoadVideoAsync(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {

                    var video = await connection.GetAsync<Video>(id);
                    return video;
                }

            }
            catch (Exception ex)
            {
                _log.LogError("Error saving", ex);
            }
            return null;
        }
    }
}
