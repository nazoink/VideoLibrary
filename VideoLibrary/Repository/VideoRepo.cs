using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;
using VideoLibrary.DataContext;
using VideoLibrary.Models;

namespace VideoLibrary.Repository
{
    /// <summary>
    /// Repository that manages persistence for <see cref="Video"/> entities using Dapper/Dapper.Contrib.
    /// </summary>
    public class VideoRepo : IVideoRepo
    {
        private readonly DapperContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<VideoRepo> _log;

        /// <summary>
        /// Creates a new instance of <see cref="VideoRepo"/>.
        /// </summary>
        public VideoRepo(IConfiguration configuration, DapperContext context,
            ILogger<VideoRepo> log)
        {
            _config = configuration;
            _context = context;
            _log = log;
        }

        /// <summary>
        /// Persists a new Video record and returns the saved entity (including generated id).
        /// Uses Dapper.Contrib's InsertAsync/GetAsync extension methods.
        /// </summary>
        public async Task<Video> SaveVideoAsync(Video videoData)
        {
            try
            {
                // Use IDbConnection returned by the context; do not rely on concrete SqlConnection type.
                using IDbConnection connection = _context.CreateConnection();

                // InsertAsync returns the new id (object) for the inserted entity.
                var id = await connection.InsertAsync(videoData);
                var video = await connection.GetAsync<Video>(id);
                return video;
            }
            catch (Exception ex)
            {
                // Log exception with structured message. Returning null indicates failure to the caller.
                _log.LogError(ex, "Error saving video");
            }

            return null;
        }

        /// <summary>
        /// Loads a Video by id, or returns null if not found / on error.
        /// </summary>
        public async Task<Video> LoadVideoAsync(int id)
        {
            try
            {
                using IDbConnection connection = _context.CreateConnection();

                var video = await connection.GetAsync<Video>(id);
                return video;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error loading video {VideoId}", id);
            }

            return null;
        }
    }
}
