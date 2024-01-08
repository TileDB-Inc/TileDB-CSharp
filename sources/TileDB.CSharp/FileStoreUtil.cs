using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB.CSharp;

public static class FileStoreUtil
{
    public static void SaveFileToArray(Context ctx, string array_uri, string file)
    {
        if (string.IsNullOrEmpty(array_uri) || string.IsNullOrEmpty(file))
        {
            return;
        }

        if (ctx == null)
        {
            Config cfg = new Config();
            ctx = new Context(cfg);
        }

        File f = new File(ctx);
        var arraySchema = f.SchemaCreate(file);
        Array.Create(ctx, array_uri, arraySchema);
        f.URIImport(
            array_uri,
            file,
            MIMEType.AutoDetect
        );
    }

    public static void SaveFileToCloudArray(Context ctx, string name_space, string array_uri, string file)
    {
        if (string.IsNullOrEmpty(name_space) || string.IsNullOrEmpty(array_uri) || string.IsNullOrEmpty(file))
        {
            return;
        }

        if (ctx == null)
        {
            Config cfg = new Config();
            ctx = new Context(cfg);
        }

        var array_name = array_uri.Split('/').Last();
        var uri_create = string.Format("tiledb://{0}/{1}", name_space, array_uri);
        var uri_write = string.Format("tiledb://{0}/{1}", name_space, array_name);

        File f = new File(ctx);
        var arraySchema = f.SchemaCreate(file);
        Array.Create(ctx, uri_create, arraySchema);
        f.URIImport(
            uri_write,
            file,
            MIMEType.AutoDetect
        );
    }

    public static void ExportArrayToFile(Context ctx, string file, string array_uri)
    {
        if (string.IsNullOrEmpty(array_uri) || string.IsNullOrEmpty(file))
        {
            return;
        }
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
