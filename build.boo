solution_file = "Nancy.Elmah.sln"
configuration = "release"
nextVersion = "0.1.1"

target default, (updateVersion, compile, nuget):
	pass

target updateVersion:  
  exec("tools\\UpdateVersion.exe", "-p ${nextVersion} -i .\\Src\\Nancy.Elmah\\Properties\\AssemblyInfo.cs -o .\\Src\\Nancy.Elmah\\Properties\\AssemblyInfo.cs")

target compile:
	msbuild(file: solution_file, configuration: configuration)

target nuget:
	exec("Tools\\NuGet.exe", "pack .\\Src\\Nancy.Elmah\\Nancy.Elmah.csproj.nuspec -Version ${nextVersion} ");