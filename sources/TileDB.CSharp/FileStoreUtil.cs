using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB.CSharp
{
    internal class FileStoreUtil
    {
        public static void SaveFileToArray(Context ctx, string array_uri, string file)
        {
            if (ctx == null)
            {
                Config cfg = new Config();
                ctx = new Context(cfg);
            }
            VFS vfs = new VFS(ctx);
            if (vfs.IsDir(array_uri))
            {
                vfs.RemoveDir(array_uri);
            }

            File f = new File(ctx);
            var arraySchema = f.SchemaCreate(file);
            Array.Create(ctx, array_uri, arraySchema);
            f.URIImport(
                array_uri,
                file,
                MIMEType.TILEDB_MIME_AUTODETECT
            );
        }

        public static void ExportArrayToFile(Context ctx, string file, string array_uri)
        {
            if (ctx == null)
            {
                Config cfg = new Config();
                ctx = new Context(cfg);
            }

            File f = new File(ctx);
            f.URIExport(
                file,
                array_uri
            );
        }
    }
}
