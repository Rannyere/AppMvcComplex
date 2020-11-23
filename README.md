# AppMvcComplex


/* Migration with Entity Framework - commands CLI */

/* ControlDbContext */
dotnet ef migrations add Initial_Data --project IO.Data -s IO.App --context ControlDbContext --verbose
dotnet ef database update Initial_Data --project IO.Data -s IO.App --context ControlDbContext --verbose

/* ApplicationDbContext */
dotnet ef migrations add Initial_Data --project IO.App -s IO.App --context ApplicationDbContext --verbose
dotnet ef database update Initial_Data --project IO.Data -s IO.App --context ApplicationDbContext --verbose