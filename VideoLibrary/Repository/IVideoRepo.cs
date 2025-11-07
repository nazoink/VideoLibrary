using System.Threading.Tasks;
using VideoLibrary.Models;

namespace VideoLibrary.Repository
{
    /// <summary>
    /// Repository contract for performing CRUD operations on <see cref="Video"/> entities.
    /// </summary>
    public interface IVideoRepo
    {
        /// <summary>
        /// Saves a video to the persistence store and returns the saved entity (including id).
        /// </summary>
        Task<Video> SaveVideoAsync(Video videoData);

        /// <summary>
        /// Loads a video by id or returns null if not found.
        /// </summary>
        Task<Video> LoadVideoAsync(int id);
    }
}
