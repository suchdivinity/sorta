# sorta

A small CLI tool to organize files into categorized folders based on file extensions.

What it does

- Scans a directory (and subfolders) for files.
- Moves files into `S<Category>` folders based on extensions.
- Creates `SOthers` for unmatched files.
- Generates a `config.json` (in the current working directory) the first time it runs. The config maps category names to extension lists so anyone can extend categories by editing this file.
- Skips files already inside folders starting with `S` and it won't move `config.json`.
- Avoids overwriting by adding numeric suffixes like `name(1).ext` when needed.

Quick start

1. Build

```bash
dotnet build
```

2. Run (example)

```bash
dotnet run -- "C:\path\to\directory"
```

Notes

- The tool writes `config.json` to the current working directory if it doesn't exist. Example structure:

```json
{
  "Categories": {
    "Pictures": [".jpg", ".png"],
    "Documents": [".pdf", ".docx"]
  }
}
```

- To add or change categories, edit `config.json` and rerun the tool.

## Thank you <3
This is my first C# project and I split the projec across small files to keep things simple and easy to work on later.


