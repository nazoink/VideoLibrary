using Dapper;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration config;
        public VideoRepo(IConfiguration configuration, DapperContext context)
        {
            config = configuration;
            _context = context;
        }

        public async Task<Video> SaveVideoAsync(Video videoData) {

            //TODO: save calls and stuff
            //var str = config.GetSection("ConnectionStrings-VideoLibrary-DB").Value;
            try
            {
                //using (SqlConnection conn = new SqlConnection(str))
                //{
                //    conn.Open();
                //    var text = $"INSERT INTO Videos (Title, CreatedBy, CreatedDate) VALUES (@Title, @CreatedBy, @CreatedDate)";

                //    using (SqlCommand cmd = conn.CreateCommand())
                //    {
                //        // Execute the command and log the # rows affected.
                //        cmd.CommandText = text;
                //        cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = videoData.Title;
                //        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = videoData.CreatedBy;
                //        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = videoData.CreatedDate;
                //        var rows = await cmd.ExecuteNonQueryAsync();
                //        //log.LogInformation($"{rows} rows were updated");
                //    }
                //}
                var query = $"INSERT INTO Videos (Title, CreatedBy, CreatedDate) VALUES (@Title, @CreatedBy, @CreatedDate)";
                using (var connection = _context.CreateConnection())
                {
                    var id = await connection.InsertAsync<Video>(videoData);
                    var video = await connection.GetAsync<Video>(id);
                    return video;
                }

            }
            catch (Exception ex)
            {
                //log.LogError("Error", ex);
            }

            return new Video();
        }
    }
}
