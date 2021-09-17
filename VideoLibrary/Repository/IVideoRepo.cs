using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary.Models;

namespace VideoLibrary.Repository
{
    public interface IVideoRepo
    {
        Task<Video> SaveVideoAsync(Video videoData);
        Task<Video> LoadVideoAsync(int id);
    }
}
