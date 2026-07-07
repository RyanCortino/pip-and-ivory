# Pip & Ivory

Pip & Ivory is a hub for tile-matching games, bringing together classic modes and original variants under one platform. Whether you're looking for traditional block and draw play, speed-focused variants, or entirely custom rulesets, Pip & Ivory provides a unified space to play, learn, and compete.

Built to be modular and extensible, the project separates core game logic, rule variants, and UI presentation, making it straightforward to add new modes without touching the underlying engine. Each mode plugs into shared systems for scoring, matchmaking, and tile management, so new game types can be prototyped quickly while staying consistent with the rest of the platform.

Whether you're a player looking for a fresh take on a familiar pastime or a contributor interested in building new modes, Pip & Ivory aims to be a welcoming home for tile-based tabletop gaming, reimagined for the screen.

## Build

Run `dotnet build` to build the solution.

## Run

To run the application:

```bash
dotnet run --project .\src\AppHost
```

The Aspire dashboard will open automatically, showing the application URLs and logs.

## Code Styles & Formatting

The template includes [EditorConfig](https://editorconfig.org/) support to help maintain consistent coding styles for multiple developers working on the same project across various editors and IDEs. The **.editorconfig** file defines the coding styles applicable to this solution.

## Code Scaffolding

The template includes support to scaffold new commands and queries.

Start in the `.\src\Application\` folder.

Create a new command:

```
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int
```

Create a new query:

```
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
```

If you encounter the error *"No templates or subcommands found matching: 'ca-usecase'."*, install the template and try again:

```bash
dotnet new install Clean.Architecture.Solution.Template::10.8.0
```

## Test

The solution contains unit, integration, and functional tests.

To run the tests:
```bash
dotnet test
```