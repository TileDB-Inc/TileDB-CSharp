# Contributing to TileDB-CSharp

Hi! Thanks for your interest in TileDB-CSharp. The following notes are intended to help you file issues, bug reports, or contribute code to the open source TileDB-CSharp project.

## Contribution Checklist

* Reporting a bug?  Please read [how to file a bug report](#reporting-a-bug) section to make sure sufficient information is included.

* Contributing code? You rock! Be sure to [review the contributor section](#contributing-code) for helpful tips on the tools we use to build TileDB-CSharp, format code, and issue pull requests (PRs).

## Reporting a Bug

A useful bug report filed as a GitHub issue provides information about how to reproduce the error.

1. Before opening a new [GitHub issue](https://github.com/TileDB-Inc/TileDB-CSharp/issues) try searching the existing issues to see if someone else has already noticed the same problem.

2. When filing a bug report, provide where possible:
  - The version of TileDB or, if a `main` version, the specific commit that triggers the error.
  - The full error message, including the stack trace (if possible).
  - A minimal working example, i.e. the smallest chunk of code that triggers the error. Ideally, this should be code that can be a small reduced C# source file. If the code to reproduce is somewhat long, consider putting it in a [gist](https://gist.github.com).

3. When pasting code blocks or output, put triple backquotes (\`\`\`) around the text so GitHub will format it nicely. Code statements should be surrounded by single backquotes (\`). See [GitHub's guide on Markdown](https://guides.github.com/features/mastering-markdown) for more formatting tricks.

## Contributing Code

*By contributing code to TileDB-CSharp, you are agreeing to release it under the [MIT License](https://github.com/TileDB-Inc/TileDB-CSharp/blob/main/LICENSE).*

### Quickstart Workflow:

1. [Fork this repository](https://help.github.com/articles/fork-a-repo/).
2. From your local machine:

    git clone https://github.com/username/TileDB-CSharp
    cd TileDB-CSharp
    git checkout -b <my_initials>/<my_bugfix_branch>
    # ... code changes ...
    git add <my_changed_files>
    git commit -m "my commit message"
    git push --set-upstream origin <my_initials>/<my_bugfix_branch>

3. [Issue a PR from your updated TileDB fork](https://help.github.com/articles/creating-a-pull-request-from-a-fork/)

Branch conventions:
- `main` is the development branch of TileDB. All PR's are merged into `main`.
- `release-x.y.z` are major/bugfix release branches of TileDB.

### Using custom native libraries

The `TileDB.CSharp` project uses [the official native binaries from NuGet](https://www.nuget.org/packages?q=TileDB.Native). You can during development provide your own native library for purposes like testing. To do that, you have to go to the `Directory.Packages.props` file of your repository, and set the `LocalLibraryFile` property to the path of your local native binary. This will bypass the standard acquisition mechanism and simply copy the libeary to your project's output directory.

**Note:** The shipped `TileDB.CSharp` NuGet package supports only the official native binaries at the moment. Please [contact us](https://tiledb.com/contact) or [open a GitHub issue](https://github.com/TileDB-Inc/TileDB-CSharp/issues/new/choose) if you want to use TileDB from C# with custom native binaries.

### Pull Requests

- `main` is the development branch, all PR’s should be rebased on top of the latest `main` commit.

- Commit changes to a local branch.  The convention is to use your initials to identify branches: (ex. “Fred Jones” , `fj/my_bugfix_branch`).  Branch names should be identifiable and reflect the feature or bug that they want to address / fix. This helps in deleting old branches later.

- When ready to submit a PR, `git rebase` the branch on top of the latest `main` commit.  Be sure to squash / cleanup the commit history so that the PR preferably one, or a couple commits at most.  Each atomic commit in a PR should be able to pass the test suite.

- Make sure that your contribution generally follows the format and naming conventions used by surrounding code.

- Submit a PR, writing a descriptive message.  If a PR closes an open issue, reference the issue in the PR message (ex. If an issue closes issue number 10, you would write `closes #10`)

- Make sure CI (continuous integration) is passing for your PR -- click `Show all checks` in the pull request status box at the bottom of each PR page.

### Resources

* TileDB
  - [Homepage](https://tiledb.com)
  - [Documentation](https://cloud.tiledb.com/academy/)
  - [Issues](https://github.com/TileDB-Inc/TileDB-CSharp/issues)
  - [Forum](https://forum.tiledb.com/)
  - [Organization](https://github.com/TileDB-Inc/)


* GitHub / Git
  - [Git cheatsheet](https://services.github.com/on-demand/downloads/github-git-cheat-sheet/)
  - [GitHub Documentation](https://help.github.com/)
  - [Forking a Repo](https://help.github.com/articles/fork-a-repo/)
  - [More Learning Resources](https://help.github.com/articles/git-and-github-learning-resources/)
