using ICSharpCode.SharpZipLib.Tar;
using System.IO;
using System.IO.Compression;
using System.Linq;
using static IPWebService.Helpers.Guard;

namespace IPWebService.Services.Geolite
{
    public sealed class GeoliteFileService : IGeoliteFileService
    {
        public string DecompressGzip(string gzipPath, bool deleteGzipFile = true)
        {
            if (string.IsNullOrEmpty(gzipPath))
                NullArgument.Throw(nameof(gzipPath));

            if (!File.Exists(gzipPath))
                throw new FileNotFoundException($"File was not found {gzipPath}");

            var gzipFileInfo = new FileInfo(gzipPath);

            // Just crop filename. Example, from "geolite.tar.gz" to "geolite.tar"
            string decompressedFileName = gzipFileInfo.FullName.Remove
                                          (gzipFileInfo.FullName.Length - gzipFileInfo.Extension.Length);


            using (var compressedFileStream = new FileStream(path: gzipPath, FileMode.Open))
            using (var decompressedFileStream = new FileStream(path: decompressedFileName, FileMode.Create))
            using (var gzipStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            {
                gzipStream.CopyTo(decompressedFileStream);
            }

            if (deleteGzipFile)
                File.Delete(gzipPath);
            return decompressedFileName;
        }


        public string ExtractDbFileFromArchive(string archivePath, bool deleteArchive = true)
        {
            if (string.IsNullOrEmpty(archivePath))
                NullArgument.Throw(nameof(archivePath));

            if (!File.Exists(archivePath))
                throw new FileNotFoundException($"Following file was not found {archivePath}");

            var archiveInfo = new FileInfo(archivePath);

            // Dir where we would like to extract our archive to
            string directoryToExtractArhive = Path.Combine(archiveInfo.DirectoryName, "GeoliteDb_Extract_Folder");
            string directoryToPutDbFile = archiveInfo.DirectoryName;


            using (var decompressedFileStream = new FileStream(path: archivePath, FileMode.Open))
            using (var tarStream = TarArchive.CreateInputTarArchive(decompressedFileStream))
                tarStream.ExtractContents(directoryToExtractArhive);

            if (deleteArchive)
                File.Delete(archivePath);


            var extractedArchive = new DirectoryInfo(directoryToExtractArhive);

            FileInfo[] dbFiles = extractedArchive.GetFiles("*.mmdb", SearchOption.AllDirectories);

            if (dbFiles.Length > 1)
                throw new FileLoadException($"More that one .mmdb exist exist in {extractedArchive}");


            FileInfo geoliteDatabase = dbFiles.FirstOrDefault();

            if (geoliteDatabase is null)
                throw new FileLoadException($"No file of type .mmdb has been found {extractedArchive}");

            string geoliteDatabaseNewPath = Path.Combine(directoryToPutDbFile, geoliteDatabase.Name);
            geoliteDatabase.MoveTo(geoliteDatabaseNewPath);

            // delete extracted folder
            Directory.Delete(directoryToExtractArhive);

            return geoliteDatabaseNewPath;
        }
    }
}
