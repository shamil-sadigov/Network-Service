using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPWebService.Services.Geolite
{
    public interface IGeoliteFileService
    {
        /// <summary>
        /// Decompress specified gzip file and returns archive
        /// </summary>
        /// <param name="gzipPath">Gzip file that need to be decompressed</param>
        /// <param name="deleteGzipFile">Delete gzip file after decompressing or ?...</param>
        /// <returns>Returns decompressed archive file path</returns>
        string DecompressGzip(string gzipPath, bool deleteGzipFile = true);

        /// <summary>
        /// Extracts db file from archive and return path to dbfile
        /// </summary>
        /// <param name="archivePath"></param>
        /// <returns>Geolite database file path </returns>
        string ExtractDbFileFromArchive(string archivePath, bool deleteArchive = true);
    }
}
