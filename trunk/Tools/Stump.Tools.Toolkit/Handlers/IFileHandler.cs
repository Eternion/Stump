using System.IO;

namespace Stump.Tools.Toolkit.Handlers
{
    public interface IFileHandler
    {
        bool Process(Stream file); 
    }
}