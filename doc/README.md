Documentation is rendered by [DocFx](https://dotnet.github.io/docfx/)

For the static site, rendered docs should *only* be pushed from the `doc` branch.

To update:

- `cd doc/docfx_project`
- `git clone https://github.com/TileDB-Inc/TileDB-CSharp -b doc docfx_project/_site`
- `sh build_doc.sh`
- `cd docfx_project/_site`
  ** Note the separate git history here!**
- `git add `docs`
- `git commit -m "update docs ..."`
- `git push origin doc`
